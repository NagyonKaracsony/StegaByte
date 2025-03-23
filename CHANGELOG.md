# StegaByte Changelog

## All notable changes to this project will be documented in this file.

> This format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),  
> and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

# [1.2.4] - 2025-03-22
### Added
- Added Support for **.NET 6.0** runtime.
- Added Support for **.NET 7.0** runtime.
- Added Support for **.NET 9.0 (preview)** runtime.
- Added [CONTRIBUTING.md](CONTRIBUTING.md).
### Changed
- Updated the Primary [README](README.md).
- Updated the Package-included [README](Src/StegaByte/Docs/README.md).
- Updated [Utility.cs](Src/StegaByte/Utility.cs)
    - Expanded the StegaByteProfile class.
    - Removed the TB unit from the DataSizeUnit enum.
### Removed
- Microsoft.Bcl.AsyncInterfaces (>= 9.0.1) dependency (not sure how it was even included)

---

# [1.0.0] - 2025-03-16
### Added
- Initial release.