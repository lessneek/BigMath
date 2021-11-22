# BigMath

Portable class library for computations with big numbers such as `Int128`, `Int256` and `BigInteger`. There is also `ExtendedBitConverter` which allows to convert regular and big numbers to/from array of bytes, with possibility of explicit setting of bytes order [Big-endian/Little-endian](http://en.wikipedia.org/wiki/Endianness).

To install BigMath, run the following command in the [Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console):

    PM> Install-Package BigMath

Supported platforms:

- .NET Framework 4.5 
- .NET for Windows Store apps
- .NET for Windows Phone 8 apps
- Portable Class Libraries

Some parts of the code is based on:

1. Original sources from [http://int128.codeplex.com/](http://int128.codeplex.com/). Thanks to [Simon Mourier](https://www.codeplex.com/site/users/view/simonm).
2. `BigInteger` from the [Bouncy Castle](http://www.bouncycastle.org/). 

### Develop builds ###
[![BigMath build status](https://www.myget.org/BuildSource/Badge/taggersoft?identifier=82116a31-a94f-4cb1-b3d7-24c01e151241)](https://www.myget.org/gallery/taggersoft)

