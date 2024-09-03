using domain.Attributes;
using JsonLocalizer;

namespace tests.domain.Attributes;

public class LanguageTests
{
    private readonly LanguageAttribute _attribute = new();

    public LanguageTests()
    {
        var mockLocalizer = new Mock<ILocalizer>();
        Localizer.SetLocalizer(mockLocalizer.Object);
    }

    [Theory]
    [InlineData("English")]
    [InlineData("German")]
    [InlineData("French")]
    [InlineData("Abaza")]
    [InlineData("Abkhazian")]
    [InlineData("Avarsky")]
    [InlineData("Azerbaijani")]
    [InlineData("Albanian")]
    [InlineData("Amharic")]
    [InlineData("Arab")]
    [InlineData("Armenian")]
    [InlineData("Basque")]
    [InlineData("Bashkir")]
    [InlineData("Belorussian")]
    [InlineData("Bengal")]
    [InlineData("Burmese")]
    [InlineData("Bulgarian")]
    [InlineData("Bosnian")]
    [InlineData("Buryat")]
    [InlineData("Hungarian")]
    [InlineData("Vietnamese")]
    [InlineData("Greek")]
    [InlineData("Georgian")]
    [InlineData("Dagestan")]
    [InlineData("Darginsky")]
    [InlineData("Danish")]
    [InlineData("Hebrew")]
    [InlineData("Ingush")]
    [InlineData("Indonesian")]
    [InlineData("Irish")]
    [InlineData("Icelandic")]
    [InlineData("Spanish")]
    [InlineData("Italian")]
    [InlineData("Kazakh")]
    [InlineData("Karelian")]
    [InlineData("Catalan")]
    [InlineData("Kashmiri")]
    [InlineData("Chinese")]
    [InlineData("Korean")]
    [InlineData("Kyrgyz")]
    [InlineData("Laksky")]
    [InlineData("Laotian")]
    [InlineData("Latin")]
    [InlineData("Latvian")]
    [InlineData("Lithuanian")]
    [InlineData("Macedonian")]
    [InlineData("Malaysian")]
    [InlineData("Mansiysk")]
    [InlineData("Mari")]
    [InlineData("Mongolian")]
    [InlineData("Nepali")]
    [InlineData("Dutch")]
    [InlineData("Nogaisky")]
    [InlineData("Norwegian")]
    [InlineData("Ossetian")]
    [InlineData("Punjabi")]
    [InlineData("Persian")]
    [InlineData("Polish")]
    [InlineData("Portuguese")]
    [InlineData("Pashto")]
    [InlineData("Romanian")]
    [InlineData("Russian")]
    [InlineData("Sanskrit")]
    [InlineData("Serbian")]
    [InlineData("Slovak")]
    [InlineData("Slovenian")]
    [InlineData("Somali")]
    [InlineData("Swahili")]
    [InlineData("Tagalog")]
    [InlineData("Tajik")]
    [InlineData("Thai")]
    [InlineData("Talyshsky")]
    [InlineData("Tamil")]
    [InlineData("Tatar")]
    [InlineData("Tibetan")]
    [InlineData("Tuvinsky")]
    [InlineData("Turkish")]
    [InlineData("Turkmen")]
    [InlineData("Udmurt")]
    [InlineData("Uzbek")]
    [InlineData("Uigur")]
    [InlineData("Ukrainian")]
    [InlineData("Urdu")]
    [InlineData("Finnish")]
    [InlineData("Flemish")]
    [InlineData("Hindi")]
    [InlineData("Croatian")]
    [InlineData("Chechen")]
    [InlineData("Czech")]
    [InlineData("Chuvash")]
    [InlineData("Swedish")]
    [InlineData("Esperanto")]
    [InlineData("Estonian")]
    [InlineData("Yakut")]
    [InlineData("Japanese")]
    public void IsValid_Success(string language)
    {
        var result = _attribute.IsValid(language);
        Assert.True(result);
    }

    [Theory]
    [InlineData("None")]
    [InlineData("Unknown")]
    public void IsValid_Fail(string language)
    {
        var result = _attribute.IsValid(language);
        Assert.False(result);
    }
}
