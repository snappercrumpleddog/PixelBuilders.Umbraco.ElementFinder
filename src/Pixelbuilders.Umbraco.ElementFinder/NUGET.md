# Element Finder

**Element Finder** is an Umbraco package that adds a dashboard to the CMS, allowing users to search for elements or blocks being used within the CMS.

No configuration is required for the package. Once installed, simply log into the CMS, and you will be presented with a new dashboard on the home screen. This will list all the available element types within a dropdown.

[![Screenshot 1](https://raw.githubusercontent.com/pixelbuilders/PixelBuilders.Umbraco.ElementFinder/refs/heads/main/images/image-1.png)](https://raw.githubusercontent.com/pixelbuilders/PixelBuilders.Umbraco.ElementFinder/refs/heads/main/images/image-1.png)

Selecting an element will display all the pages where the element can be found, with information on the page name, path, and URL.

[![Screenshot 2](https://raw.githubusercontent.com/pixelbuilders/PixelBuilders.Umbraco.ElementFinder/refs/heads/main/images/image-2.png)](https://raw.githubusercontent.com/pixelbuilders/PixelBuilders.Umbraco.ElementFinder/refs/heads/main/images/image-2.png)

The package also allows you to quickly view and edit the content of the pages.

[![Screenshot 3](https://raw.githubusercontent.com/pixelbuilders/PixelBuilders.Umbraco.ElementFinder/refs/heads/main/images/image-3.png)](https://raw.githubusercontent.com/pixelbuilders/PixelBuilders.Umbraco.ElementFinder/refs/heads/main/images/image-3.png)

## Requirements

Currently, this package is only compatible with Umbraco 13. It can be installed on any on-premises or Umbraco Cloud installation.

## Installation

### Visual Studio

Simply search for the `Pixelbuilders.Umbraco.ElementFinder` NuGet package and add it to your project.

### CLI

`dotnet add package Pixelbuilders.Umbraco.ElementFinder`