#!/bin/bash

# Colors for output
RED='\033[0;31m'
GREEN='\033[0;32m'
YELLOW='\033[1;33m'
NC='\033[0m' # No Color

# Function to display usage
usage() {
    echo "Usage: $0 [patch|minor|major] [--dry-run]"
    echo "  patch: Increment patch version (x.y.z -> x.y.z+1)"
    echo "  minor: Increment minor version (x.y.z -> x.y+1.0)"
    echo "  major: Increment major version (x.y.z -> x+1.0.0)"
    echo "  --dry-run: Show what would be done without making changes"
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
INCREMENT_TYPE="patch"

for arg in "$@"; do
    case $arg in
        --dry-run)
            DRY_RUN=true
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

if [ "$DRY_RUN" = true ]; then
    echo -e "${GREEN}🔍 DRY RUN SUMMARY:${NC}"
    echo -e "${GREEN}  - Current version: ${CURRENT_VERSION}${NC}"
    echo -e "${GREEN}  - New version: ${NEW_VERSION}${NC}"
    echo -e "${GREEN}  - Tag to create: ${NEW_TAG}${NC}"
    echo -e "${GREEN}  - Commands that would run:${NC}"
    echo -e "${GREEN}    git tag ${NEW_TAG}${NC}"
    echo -e "${GREEN}    git push origin ${NEW_TAG}${NC}"
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

# Create and push the tag
if git tag "$NEW_TAG"; then
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

echo -e "${GREEN}🎉 Release process completed!${NC}"
echo -e "${GREEN}📦 GitHub Actions will now build and publish the NuGet package${NC}"

# Try to get direct link to the workflow run (best effort)
echo -e "${YELLOW}🔍 Looking for triggered workflow...${NC}"
sleep 3  # Give GitHub a moment to register the workflow

if command -v gh >/dev/null 2>&1; then
    # Get the most recent workflow run (should be ours)
    RUN_URL=$(gh run list --limit 1 --json url --jq '.[0].url' 2>/dev/null)
    if [[ -n "$RUN_URL" && "$RUN_URL" != "null" ]]; then
        echo -e "${YELLOW}📋 Direct link to workflow: ${RUN_URL}${NC}"
    else
        echo -e "${YELLOW}📋 You can monitor the progress at: https://github.com/maximn/google-maps/actions${NC}"
    fi
else
    echo -e "${YELLOW}📋 You can monitor the progress at: https://github.com/maximn/google-maps/actions${NC}"
fi