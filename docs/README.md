# Documentation

This directory contains all project documentation organized by category.

## 📁 Directory Structure

### `/architecture/`
High-level architectural decisions, roadmaps, and design documents.
- `MODERNIZATION_ROADMAP.md` - Comprehensive modernization strategy and implementation plan

### `/analysis/`
Technical analysis documents, gap analysis, and feature comparisons.
- `google-maps-api-gaps-analysis.md` - Analysis of missing Google Maps API features

### `/reports/`
Generated reports, analysis outputs, and build artifacts.
- `nullable_analysis_report.md` - Nullable reference types analysis
- `nullable_warnings_*.txt` - Compiler warning reports

### `/guides/`
User guides, tutorials, and how-to documentation.
- *Currently empty - add user guides here*

### `/api/`
API documentation, specifications, and reference materials.
- *Future: OpenAPI specs, endpoint documentation*

## 📋 Documentation Standards

### File Naming Conventions
- Use kebab-case for file names: `modernization-roadmap.md`
- Use descriptive names that indicate content type
- Include version numbers for major documents: `api-v2-specification.md`

### Document Structure
- Start with a clear title and executive summary
- Use consistent heading hierarchy (H1 for title, H2 for main sections)
- Include table of contents for long documents
- Add last updated date in document footer

### Content Guidelines
- Write for your target audience (developers, architects, users)
- Include code examples where relevant
- Use emojis sparingly for visual hierarchy
- Keep documents up-to-date with code changes

## 🔄 Maintenance

- Review and update documentation with each major release
- Archive outdated documents in `/archive/` subdirectory
- Use version control to track documentation changes
- Link related documents with cross-references

## 📚 Related Documentation

- [Main README](../README.md) - Project overview and quick start
- [Release Notes](../GoogleMapsApi/ReleaseNotes.md) - Version history
- [Security Policy](../SECURITY.md) - Security guidelines
- [CLAUDE.md](../CLAUDE.md) - AI assistant interaction guidelines