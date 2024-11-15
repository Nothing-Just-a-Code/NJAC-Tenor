# NJAC-Tenor
A Very Simple and Easy to Use Tenor Library.

![Static Badge](https://img.shields.io/badge/Stable%20Release-1.0.0-blue) ![Static Badge](https://img.shields.io/badge/Open%20Source-8A2BE2)

## Table of Contents
- [About](#about)
- [Features](#features)
- [Installation](#installation)
- [Usage](#usage)
- [License](#license)

## About
This is a simple and very easy-to-use UnOfficial Tenor Library for .NET Apps. It has all the required features if you're looking for a library to develop any bot or application that needs Tenor Images.

## Features
- Get GIF Images.
- Get Stickers (Animated/Static or Both).
- Can set the search items limit to avoid overload and for quick response.
- Get Categories and Items based on searched categories without any extra coding.
- Can get images with multiple formats ( .gif | .webp | .webm | .png (Transparent for Stickers) )
- Get AutoComplete Suggestions.

### Prerequisites
- Newtonsoft.Json
- .NET 8 (or lower)

### Installation
- Clone the repo:
   ```bash
   git clone https://github.com/Nothing-Just-a-Code/NJAC-Tenor.git
  ```
or
- Install from NuGet Package Manager:
  ```bash
    Install-Package NJACTenor -Version 1.0.0
  ```

  
---

## Usage

- Create a New Client
```vb.net
Imports NJACTenor.TenorClient
Private Client as New TenorClient("YOUR_TENOR_API_KEY_HERE)
```

- To Search GIF Images
```vb.net
Dim images = Await client.GIFSearch.SearchGIF("hello")
 For Each item As ImageItem In images.Images
     PictureBox1.LoadAsync(item.GIF)
 Next
```

- To Search Stickers
```vb.net
 Dim Stickers = Await client.GIFSearch.SearchSticker("laugh")
 For Each item As StickerItem In Stickers.Stickers
     PictureBox1.LoadAsync(item.GIFTransparent)
 Next
```

- To Get AutoComplete Search Items
  ```vb.net
  Dim searchsuggestions = Await client.AutoCompleteSuggestions("good")
  ' It will return as result with search items string collection
  ' For example: good morning, good night, good day
  ```

- Get Categories and its Items and Info.
 ```vb.net
 Dim Categories = Await client.GetCategories(TenorClient.CategoryType.Featured)
   For Each item As CategoryItem In Categories
      'item.name - Category Name 
      'item.SearchTerm - Query text which going to be used to search images in that Category
      'item.Query - A full query contains text and all the params to search image in that Category
      'item.Image - Thumbnail image of that Category
      'Dim getimages = Await item.GetItems() - Get all images inside that Category.
   Next
 ```

## License
This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.


