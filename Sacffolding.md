# RazorPagesMovie

Razor에서 Scaffolding을 통해 만든 Razor 페이지 탐색


```C# 
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
    public class IndexModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IList<Movie> Movie { get; set; }

        public async Task OnGetAsync()
        {
            Movie = await _context.Movie.ToListAsync();
        }
    }
}
```
Razor Pages는 PageModel에서 파생됩니다. 일반적으로 PageModel 파생클래스의 이름은 Page Name Model로 지정된다.
예를 들어 Index 페이지의 이름은 IndexModel로 지정된다.

생성자는 종속성 주입을 사용하여 페이지에 추가한다

```C#
public class IndexModel : PageModel
{
    private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

    public IndexModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
    {
        _context = context;
    }
```
Entity Framework로 비동기 프로그래밍에 대한 자세한 내용은 [비동기 코드](https://docs.microsoft.com/ko-kr/aspnet/core/data/ef-rp/intro?view=aspnetcore-6.0&tabs=visual-studio#asynchronous-code)를 참조하세요.

페이지에 대한 요청을 만들면 OnGetAsync 메서드가 Razor Page에 동영상 목록을 반환합니다. Razor 페이지에서 OnGetAsync 또는 OnGet을 호출하여 페이지 상태를 초기화합니다. 이 경우 OnGetAsync는 동영상 목록을 가져와 표시합니다.

OnGet이 void를 반환하거나 OnGetAsync가 Task를 반환하면 return 문이 사용되지 않은 것입니다. 예를 들어 Privacy 페이지를 검토합니다.
```C#
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorPagesMovie.Pages;
public class PrivacyModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;

    public PrivacyModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }
}
```
반환 형식이 [IActionResult](https://docs.microsoft.com/ko-KR/dotnet/api/microsoft.aspnetcore.mvc.iactionresult?view=aspnetcore-6.0)  또는 Task<IActionResult>이면 반환 문을 제공해야 합니다. Pages/Movies/Create.cshtml.cs 메서드를 예로 들 수 있습니다.

 ``` C#
public async Task<IActionResult> OnPostAsync()
{
    if (!ModelState.IsValid)
    {
        return Page();
    }

    _context.Movie.Add(Movie);
    await _context.SaveChangesAsync();

    return RedirectToPage("./Index");
}
  ```
  
  ``` CSHTML
  @page
@model RazorPagesMovie.Pages.Movies.IndexModel

@{
    ViewData["Title"] = "Index";
}

<h1>Index</h1>

<p>
    <a asp-page="Create">Create New</a>
</p>
<table class="table">
    <thead>
        <tr>
            <th>
                @Html.DisplayNameFor(model => model.Movie[0].Title)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.Movie[0].ReleaseDate)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.Movie[0].Genre)
            </th>
            <th>
                @Html.DisplayNameFor(model => model.Movie[0].Price)
            </th>
            <th></th>
        </tr>
    </thead>
    <tbody>
@foreach (var item in Model.Movie) {
        <tr>
            <td>
                @Html.DisplayFor(modelItem => item.Title)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.ReleaseDate)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.Genre)
            </td>
            <td>
                @Html.DisplayFor(modelItem => item.Price)
            </td>
            <td>
                <a asp-page="./Edit" asp-route-id="@item.ID">Edit</a> |
                <a asp-page="./Details" asp-route-id="@item.ID">Details</a> |
                <a asp-page="./Delete" asp-route-id="@item.ID">Delete</a>
            </td>
        </tr>
}
    </tbody>
</table>
  ```
  
  Razor는 HTML에서 C# 또는 Razor 관련 태그로 전환될 수 있습니다. @ 기호 뒤에 @가 사용되면 이 기호는 Razor 관련 태그로 전환됩니다. 이외의 경우에는 C#으로 전환됩니다.

@page 지시문
  ----------
@pageRazor 지시문은 파일을 MVC 작업으로 만듭니다. 이는 요청을 처리할 수 있음을 의미합니다. @page는 페이지의 첫 번째 Razor 지시문이어야 합니다. @page 및 @model은 Razor 관련 태그로 전환되는 예입니다. 자세한 내용은 구문을 참조하세요.
  
  @model 지시문 
  ----------
  ```CSHTML
  @page
@model RazorPagesMovie.Pages.Movies.IndexModel
  ```
  @model 지시문은 Razor Page에 전달되는 모델 형식을 지정합니다. 위의 예제에서 @model 줄은 Razor Page에서 PageModel 파생 클래스를 사용할 수 있게 만듭니다. 모델은 페이지에서 @Html.DisplayNameFor 및 @Html.DisplayFor@Html.DisplayNameFor에서 사용됩니다.

다음 HTML 도우미에서 사용되는 람다 식을 살펴봅니다.
  
  @Html.DisplayNameFor(model => model.Movie[0].Title)
  
  [DisplayNameFor](https://docs.microsoft.com/ko-KR/dotnet/api/microsoft.aspnetcore.mvc.rendering.ihtmlhelper-1.displaynamefor?view=aspnetcore-6.0) HTML 도우미는 람다 식에서 참조되는 Title 속성을 검사하여 표시 이름을 확인합니다. 
  람다 식은 계산되는 것이 아니라 검사됩니다. 즉, model, model.Movie 또는 model.Movie[0]가 null이거나 비어 있을 경우 액세스 위반이 없습니다. 
  람다 식이 계산될 경우(예: @Html.DisplayFor(modelItem => item.Title) 사용) 모델의 속성 값이 계산됩니다.
  
  
레이아웃 페이지
  ----------
  메뉴 링크 PagesMovie, Home 및 Privacy를 선택합니다. 각 페이지는 동일한 메뉴 레이아웃을 보여줍니다. 메뉴 레이아웃은 Pages/Shared/_Layout.cshtml 파일에서 구현됩니다.

Pages/Shared/_Layout.cshtml 파일을 열고 검사합니다.

[레이아웃](https://docs.microsoft.com/ko-kr/aspnet/core/mvc/views/layout?view=aspnetcore-6.0) 템플릿을 사용하여 HTML 컨테이너 레이아웃을 다음과 같이 지정할 수 있습니다.

- 한 위치에 지정됩니다.
- 사이트의 여러 페이지에 적용됩니다.
@RenderBody() 줄을 찾습니다. RenderBody는 사용자가 만드는 모든 페이지 특정 보기가 표시되는 자리 표시자이며 레이아웃 페이지에서 RenderBody됩니다. 예를 들어, Privacy 링크를 선택하는 경우 Privacy 보기는 RenderBody 메서드 내에서 렌더링됩니다.


ViewData
  
ViewData 및 레이아웃
  ----------
  @page
@model RazorPagesMovie.Pages.Movies.IndexModel

  @{
      ViewData["Title"] = "Index";
  }
  강조 표시된 이전 태그는 C#으로 전환되는 Razor의 예제입니다. { 및 } 문자로 C# 코드 블록을 묶습니다.

PageModel 기본 클래스에는 데이터를 뷰에 전달하는 데 사용할 수 있는 ViewData 사전 속성이 있습니다. ViewData 패턴을 사용하여 개체가 ViewData 사전에 추가됩니다. 이전 샘플에서는 Title 속성이 ViewData 사전에 추가됩니다.

Title 속성은 Title 파일에서 사용됩니다. 다음 태그는 _Layout.cshtml 파일의 처음 몇 줄을 표시합니다.
  
  <!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Movie</title>
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
  
  @*Markup removed for brevity.*@ 줄은 Razor 주석입니다. HTML 주석 <!-- -->과 달리 Razor 주석은 클라이언트에 전송되지 않습니다. 
  자세한 내용은 [MDN 웹 문서: HTML 시작](https://developer.mozilla.org/ko/docs/Learn/HTML/Introduction_to_HTML/Getting_started#HTML_comments)을 참조하세요.
  
  레이아웃 업데이트
  ----------
    1. <title> 파일의 <title> 요소를 변경하여 PagesMovie 대신 Movie를 표시합니다.
  <!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Movie</title>
  
    2. Pages/Shared/_Layout.cshtml 파일에서 다음 앵커 요소를 찾습니다.
  <a class="navbar-brand" asp-area="" asp-page="/Index">RazorPagesMovie</a>
    
    3. 이전 요소를 다음 태그로 바꿉니다.
  <a class="navbar-brand" asp-page="/Movies/Index">RpMovie</a>
  이전 앵커 요소는 태그 도우미입니다. 이 경우에는 앵커 태그 도우미입니다. 
  asp-page="/Movies/Index" 태그 도우미 특성 및 값으로 /Movies/IndexRazor 페이지의 링크를 만듭니다. 
  sp-area 특성 값이 비어 있으므로 영역은 링크에서 사용되지 않습니다. 자세한 내용은 영역을 참조하세요.
  
    4.변경 내용을 저장하고 RpMovie 링크를 선택하여 앱을 테스트합니다. 문제가 있는 경우 GitHub에서 [_Layout.cshtml](https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/tutorials/razor-pages/razor-pages-start/sample/RazorPagesMovie60/Pages/Shared/_Layout.cshtml) 파일을 참조하세요.
  
  
    5. Home, Home, 만들기, 편집 및 삭제 링크를 테스트합니다. 
  각 페이지에서 설정되는 제목은 브라우저 탭에서 확인할 수 있습니다. 페이지의 책갈피를 지정하면 제목이 책갈피에 사용됩니다.
  
  
    Price 필드에 소수점을 입력하지 못할 수도 있습니다. 
    소수점으로 쉼표(",")를 사용하는 비영어 로캘 및 비미국 영어 날짜 형식에 대해 jQuery 유효성 검사를 지원하려면 앱을 세계화하는 단계를 수행해야 합니다. 
    소수점 추가에 대한 지침은 이 GitHub 문제 4076을 참조하세요.
 
 Layout 속성은 Layout 파일에서 설정됩니다.
  
  @{
    Layout = "_Layout";
}
  
  
  이전 태그는 Pages 폴더 아래에 있는 모든 파일에 대한 레이아웃 파일을 Pages/Shared/_Layout.cshtml로 설정합니다. 자세한 내용은 [레이아웃](https://docs.microsoft.com/ko-kr/aspnet/core/razor-pages/?view=aspnetcore-6.0&tabs=visual-studio#layout)을 참조하세요.
    
  Create 페이지 모델
  ----------
  Pages/Movies/Create.cshtml.cs 페이지 모델을 살펴봅니다.
  
  using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagesMovie.Models;

namespace RazorPagesMovie.Pages.Movies
{
#pragma warning disable CS8618
#pragma warning disable CS8602

    public class CreateModel : PageModel
    {
        private readonly RazorPagesMovie.Data.RazorPagesMovieContext _context;

        public CreateModel(RazorPagesMovie.Data.RazorPagesMovieContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Movie Movie { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Movie.Add(Movie);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
#pragma warning restore CS8618
#pragma warning restore CS8602
}
  
  
 OnGet 메서드는 페이지에 필요한 상태를 초기화합니다. 만들기 페이지에는 초기화할 상태가 없습니다. 따라서 Page가 반환됩니다. 
자습서의 뒷부분에서 상태를 초기화하는 OnGet의 예가 나와 있습니다. Page 메서드는 Page 페이지를 렌더링하는 PageResult 개체를 만듭니다.

Movie 속성은 Movie 특성을 사용하여 [모델 바인딩](https://docs.microsoft.com/ko-kr/aspnet/core/mvc/models/model-binding?view=aspnetcore-6.0)을 옵트인합니다. 만들기 폼이 폼 값을 게시하면 ASP.NET Core 런타임이 게시된 값을 Movie 모델에 바인딩합니다.

페이지에 폼 데이터가 게시되면 OnPostAsync 메서드가 실행됩니다.
  
  public async Task<IActionResult> OnPostAsync()
{
    if (!ModelState.IsValid)
    {
        return Page();
    }

    _context.Movie.Add(Movie);
    await _context.SaveChangesAsync();

    return RedirectToPage("./Index");
}
  
  모델 오류가 있는 경우 폼과 게시된 모든 폼 데이터가 다시 표시됩니다. 
  대부분의 모델 오류는 폼이 게시되기 전에 클라이언트 쪽에서 catch할 수 있습니다. 
  예를 들어 데이터로 변환될 수 없는 날짜 필드에 대한 값을 게시하는 모델 오류가 발생할 수 있습니다. 
  클라이언트 쪽 유효성 검사 및 모델 유효성 검사는 자습서의 뒷부분에서 설명합니다.

모델 오류가 없는 경우:
  1. 데이터가 저장됩니다.
  2. 브라우저가 Index 페이지로 리디렉션 됩니다.
  
      
  Create Razor Page
  ----------
  Pages/Movies/Create.cshtml Page 파일을 살펴봅니다.
  @page
@model RazorPagesMovie.Pages.Movies.CreateModel

@{
    ViewData["Title"] = "Create";
}

<h1>Create</h1>

<h4>Movie</h4>
<hr />
<div class="row">
    <div class="col-md-4">
        <form method="post">
            <div asp-validation-summary="ModelOnly" class="text-danger"></div>
            <div class="form-group">
                <label asp-for="Movie.Title" class="control-label"></label>
                <input asp-for="Movie.Title" class="form-control" />
                <span asp-validation-for="Movie.Title" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="Movie.ReleaseDate" class="control-label"></label>
                <input asp-for="Movie.ReleaseDate" class="form-control" />
                <span asp-validation-for="Movie.ReleaseDate" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="Movie.Genre" class="control-label"></label>
                <input asp-for="Movie.Genre" class="form-control" />
                <span asp-validation-for="Movie.Genre" class="text-danger"></span>
            </div>
            <div class="form-group">
                <label asp-for="Movie.Price" class="control-label"></label>
                <input asp-for="Movie.Price" class="form-control" />
                <span asp-validation-for="Movie.Price" class="text-danger"></span>
            </div>
            <div class="form-group">
                <input type="submit" value="Create" class="btn btn-primary" />
            </div>
        </form>
    </div>
</div>

<div>
    <a asp-page="Index">Back to List</a>
</div>

@section Scripts {
    @{await Html.RenderPartialAsync("_ValidationScriptsPartial");}
}
  
  ![image](https://user-images.githubusercontent.com/34930947/157788256-61de1e22-5b2b-4cf8-b884-3a6cc38e3946.png)

  <form method="post"> 요소는 <form method="post">입니다. 폼 태그 도우미에는 [위조 방지 토큰](https://docs.microsoft.com/ko-kr/aspnet/core/security/anti-request-forgery?view=aspnetcore-6.0)이 자동으로 포함됩니다.
    
    스캐폴딩 엔진은 ID를 제외하고 다음과 비슷한 모델에서 필드마다 Razor 태그를 만듭니다.
    
    <div asp-validation-summary="ModelOnly" class="text-danger"></div>
<div class="form-group">
    <label asp-for="Movie.Title" class="control-label"></label>
    <input asp-for="Movie.Title" class="form-control" />
    <span asp-validation-for="Movie.Title" class="text-danger"></span>
</div>
    
[유효성 검사 태그 도우미](https://docs.microsoft.com/ko-kr/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-6.0#the-validation-tag-helpers) ( 및 <span asp-validation-for) 는 유효성 검사 오류를 표시합니다. 유효성 검사는 이 시리즈의 뒷부분에서 자세히 설명합니다.

[레이블 태그 도우미](https://docs.microsoft.com/ko-kr/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-6.0#the-label-tag-helper)는 Title 속성에 대한 레이블 캡션 및 [for] 특성을 생성합니다.

[입력 태그 도우미](https://docs.microsoft.com/ko-kr/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-6.0)는 [DataAnnotations](https://docs.microsoft.com/ko-KR/aspnet/mvc/overview/older-versions/mvc-music-store/mvc-music-store-part-6)특성을 사용하고 클라이언트 쪽의 jQuery 유효성 검사에 필요한 HTML 특성을 생성합니다.

태그 도우미]=(예: <form method="post">)에 대한 자세한 내용은 <form method="post">를 참조하세요.
