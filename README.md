## AigioSoft.Common `.netstandard1.3/.net45/.net47`
AigioSoft Common .NET Standard Library
* Namespace AigioSoft.Common
  * Extensions
    * [DateTime](#datetimeextension)
    * [Enum](#enumextensions)
    * [Linq](#linqextension)
    * [String](#stringextension)
    * XDocument
* Namespace AigioSoft.Common.Helpers
  * [Hashs](#hashshelper)
  * HttpClient
  * IPHelper
  * RSA
  * VerifyCodePackage
  * ZipFile

<!--## AigioSoft.Common.Mvc `netstandard1.6`
AigioSoft Common AspNetCore.Mvc .NET Standard Library
#### Namespace
- AigioSoft.Common
- AigioSoft.Common.Helpers-->

___

### <span id="datetimeextension">DateTimeExtension</span>
```
using AigioSoft.Common;

DateTime.Now.ToStandardString();
// 2017-01-01 00:00:00

DateTime.Now.ToDateFormat();
// 2017-01-01

DateTime.Now.ToNoYearNoSecondFormat();
// 01-01 00:00

DateTime.Now.ToConnectFormat();
// 201701011230591234567

DateTime.Now.ToCompleteFormat();
// 2017-01-01 12:30:59.1234567

DateTime.Now.ToMicrosoftDateFormat();
// /Date(1492317727431+0800)/
```

### <span id="enumextensions">EnumExtensions</span>
```
using AigioSoft.Common;
using System.ComponentModel;

    public enum Enum1
    {
        [Description("TestDescription")]
        Test = 0
    }

Enum1.Test.ToDescriptionString()
// TestDescription
```

### <span id="linqextension">LinqExtension</span>
```
using AigioSoft.Common;
using System.Linq;

new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 }.AsQueryable().OrderByDescending(x => x).Paging(1, 5);
// {"PageSize":5,"Total":9,"PageCount":2,"CurrentPageIndex":1,"Table":[9,8,7,6,5]}
```

### <span id="stringextension">StringExtension</span>
```
using AigioSoft.Common;

DateTime temp = "2017-01-01".GetDateTime().Value;
int temp = "233".GetInt32().Value;
```

### <span id="hashshelper">HashsHelper</span>
```
using Helpers = AigioSoft.Common.Helpers;

Helpers.Hashs.MD5("123");
// 202cb962ac59075b964b07152d234b70

Helpers.Hashs.MD5("123",false);
// 202CB962AC59075B964B07152D234B70

Helpers.Hashs.SHA1("123");
// 40bd001563085fc35165329ea1ff5c5ecbdbbeef

Helpers.Hashs.SHA256("123");
// a665a45920422f9d417e4867efdc4fb8a04a1f3fff...

Helpers.Hashs.SHA512("123");
// 3c9909afec25354d551dae21590bb26e38d53f2173...
```
