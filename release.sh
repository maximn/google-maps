#!/bin/bash

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to display usage
usage() {
    echo "Usage: $0 [patch|minor|major] [--dry-run] [--no-notes]"
    echo "  patch: Increment patch version (x.y.z -> x.y.z+1)"
    echo "  minor: Increment minor version (x.y.z -> x.y+1.0)"
    echo "  major: Increment major version (x.y.z -> x+1.0.0)"
    echo "  --dry-run: Show what would be done without making changes"
    echo "  --no-notes: Skip release notes generation and GitHub Release creation"
    echo "  --edit: Open generated notes in \$EDITOR before accepting"
    echo "  If no argument provided, defaults to patch"
    exit 1
}

# Function to increment version
increment_version() {
    local version=$1
    local type=$2
    
    IFS='.' read -ra ADDR <<< "$version"
    local major=${ADDR[0]}
    local minor=${ADDR[1]}
    local patch=${ADDR[2]}
    
    case $type in
        "major")
            echo "$((major + 1)).0.0"
            ;;
        "minor")
            echo "${major}.$((minor + 1)).0"
            ;;
        "patch"|"")
            echo "${major}.${minor}.$((patch + 1))"
            ;;
        *)
            echo "Invalid version type: $type" >&2
            exit 1
            ;;
    esac
}

# Parse command line arguments
DRY_RUN=false
NO_NOTES=false
EDIT_NOTES=false
INCREMENT_TYPE="patch"

for arg in "$@"; do
    case $arg in
        --dry-run)
            DRY_RUN=true
            ;;
        --no-notes)
            NO_NOTES=true
            ;;
        --edit)
            EDIT_NOTES=true
            ;;
        patch|minor|major)
            INCREMENT_TYPE="$arg"
            ;;
        *)
            echo -e "${RED}Error: Invalid argument '$arg'${NC}"
            usage
            ;;
    esac
done

# Verify required tools when generating notes
if [ "$NO_NOTES" = false ]; then
    MISSING_TOOLS=()
    command -v claude >/dev/null 2>&1 || MISSING_TOOLS+=("claude")
    command -v gh >/dev/null 2>&1 || MISSING_TOOLS+=("gh")
    if [ ${#MISSING_TOOLS[@]} -gt 0 ]; then
        echo -e "${RED}Error: missing required tools: ${MISSING_TOOLS[*]}${NC}"
        echo -e "${YELLOW}Re-run with --no-notes to skip release notes generation.${NC}"
        exit 1
    fi
fi

if [ "$DRY_RUN" = true ]; then
    echo -e "${YELLOW}🚀 Starting release process (DRY RUN)...${NC}"
else
    echo -e "${YELLOW}🚀 Starting release process...${NC}"
fi

# Check if we're in a git repository
if ! git rev-parse --git-dir > /dev/null 2>&1; then
    echo -e "${RED}Error: Not in a git repository${NC}"
    exit 1
fi

# Check if working directory is clean (skip in dry run)
if [ "$DRY_RUN" = false ] && ! git diff-index --quiet HEAD --; then
    echo -e "${RED}Error: Working directory is not clean. Please commit or stash your changes.${NC}"
    exit 1
fi

# Get the latest version tag
LATEST_TAG=$(git tag --sort=-version:refname | head -n1)

if [[ -z "$LATEST_TAG" ]]; then
    echo -e "${RED}Error: No version tags found${NC}"
    exit 1
fi

# Remove 'v' prefix if present
CURRENT_VERSION=${LATEST_TAG#v}

echo -e "${YELLOW}Current version: ${CURRENT_VERSION}${NC}"

# Calculate new version
NEW_VERSION=$(increment_version "$CURRENT_VERSION" "$INCREMENT_TYPE")
NEW_TAG="v${NEW_VERSION}"

echo -e "${YELLOW}New version: ${NEW_VERSION}${NC}"
echo -e "${YELLOW}New tag: ${NEW_TAG}${NC}"

# Generate release notes
NOTES_FILE=""
if [ "$NO_NOTES" = false ]; then
    NOTES_FILE=$(mktemp -t release-notes-XXXXXX.md)
    trap 'rm -f "$NOTES_FILE"' EXIT

    echo -e "${YELLOW}✍️  Generating release notes for ${LATEST_TAG}..HEAD with claude -p...${NC}"

    COMMIT_LOG=$(git log --pretty=format:"- %s" "${LATEST_TAG}..HEAD")
    if [[ -z "$COMMIT_LOG" ]]; then
        echo -e "${RED}Error: no commits between ${LATEST_TAG} and HEAD${NC}"
        exit 1
    fi

    PROMPT="Generate concise release notes in GitHub-flavored markdown for version ${NEW_VERSION} of a .NET library (Google Maps API wrapper).

Group commits under these H2 sections, omitting any section with no entries:
## Features
## Bug fixes
## Dependencies
## Other

Rules:
- Use short, user-facing bullets. Strip conventional-commit prefixes (feat:, fix:, chore:, etc.).
- Do not include commit hashes, PR numbers, or a title heading.
- If a commit is purely internal (CI, formatting), put it under Other or omit.

Commits:
${COMMIT_LOG}"

    if ! claude -p "$PROMPT" > "$NOTES_FILE"; then
        echo -e "${RED}❌ claude -p failed to generate release notes${NC}"
        exit 1
    fi

    if [ "$DRY_RUN" = false ]; then
        if [ "$EDIT_NOTES" = true ]; then
            ${EDITOR:-vi} "$NOTES_FILE"
            if [ ! -s "$NOTES_FILE" ]; then
                echo -e "${RED}Release cancelled (empty notes)${NC}"
                exit 0
            fi
        else
            echo -e "${YELLOW}--- Generated release notes ---${NC}"
            cat "$NOTES_FILE"
            echo -e "${YELLOW}--- end notes ---${NC}"
            read -p "Accept these notes? (y/N): " -n 1 -r
            echo
            if [[ ! $REPLY =~ ^[Yy]$ ]]; then
                echo -e "${YELLOW}Release cancelled${NC}"
                exit 0
            fi
        fi
    fi
fi

if [ "$DRY_RUN" = true ]; then
    echo -e "${GREEN}🔍 DRY RUN SUMMARY:${NC}"
    echo -e "${GREEN}  - Current version: ${CURRENT_VERSION}${NC}"
    echo -e "${GREEN}  - New version: ${NEW_VERSION}${NC}"
    echo -e "${GREEN}  - Tag to create: ${NEW_TAG}${NC}"
    echo -e "${GREEN}  - Commands that would run:${NC}"
    if [ "$NO_NOTES" = false ]; then
        echo -e "${GREEN}    git tag -a ${NEW_TAG} -F <notes>${NC}"
    else
        echo -e "${GREEN}    git tag ${NEW_TAG}${NC}"
    fi
    echo -e "${GREEN}    git push origin ${NEW_TAG}${NC}"
    if [ "$NO_NOTES" = false ]; then
        echo -e "${GREEN}    gh release create ${NEW_TAG} -F <notes> --title ${NEW_TAG}${NC}"
        echo -e "${YELLOW}--- Generated release notes preview ---${NC}"
        cat "$NOTES_FILE"
        echo -e "${YELLOW}--- end preview ---${NC}"
    fi
    echo -e "${YELLOW}📦 This would trigger GitHub Actions to build and publish to NuGet${NC}"
    echo -e "${YELLOW}📋 Monitor progress at: https://github.com/maximn/google-maps/actions${NC}"
    exit 0
fi

# Confirm with user
read -p "Do you want to create and push tag $NEW_TAG? (y/N): " -n 1 -r
echo
if [[ ! $REPLY =~ ^[Yy]$ ]]; then
    echo -e "${YELLOW}Release cancelled${NC}"
    exit 0
fi

echo -e "${YELLOW}📝 Creating and pushing tag...${NC}"

# Create and push the tag (annotated when we have notes, so workflow/git can inspect them later)
if [ "$NO_NOTES" = false ]; then
    TAG_OK=$(git tag -a "$NEW_TAG" -F "$NOTES_FILE" && echo ok || echo fail)
else
    TAG_OK=$(git tag "$NEW_TAG" && echo ok || echo fail)
fi

if [ "$TAG_OK" = "ok" ]; then
    echo -e "${GREEN}✅ Tag $NEW_TAG created successfully${NC}"
else
    echo -e "${RED}❌ Failed to create tag $NEW_TAG${NC}"
    exit 1
fi

if git push origin "$NEW_TAG"; then
    echo -e "${GREEN}✅ Tag $NEW_TAG pushed successfully${NC}"
else
    echo -e "${RED}❌ Failed to push tag $NEW_TAG${NC}"
    exit 1
fi

# Create GitHub Release
if [ "$NO_NOTES" = false ]; then
    echo -e "${YELLOW}🚀 Creating GitHub Release...${NC}"
    if gh release create "$NEW_TAG" --title "$NEW_TAG" --notes-file "$NOTES_FILE"; then
        echo -e "${GREEN}✅ GitHub Release $NEW_TAG created${NC}"
    else
        echo -e "${RED}⚠️  Failed to create GitHub Release (tag is already pushed). Create manually with:${NC}"
        echo -e "${YELLOW}    gh release create $NEW_TAG --title $NEW_TAG --notes-file $NOTES_FILE${NC}"
        exit 1
    fi
fi

echo -e "${GREEN}🎉 Release process completed!${NC}"
echo -e "${GREEN}📦 GitHub Actions will now build and publish the NuGet package${NC}"
echo -e "${YELLOW}📋 You can monitor the progress at: https://github.com/maximn/google-maps/actions${NC}"
