using Utils.Tokens;

namespace TechInsightTest.Utils.Tokens;

[TestClass]
public class TokenTests
{
    [TestMethod]
    public void TestToken()
    {
        var token1 = Token.GenerateToken("lll", "asdsad");
        Assert.IsTrue(Token.ValidateToken(token1, "lll", "asdsad"));

        var token2 = Token.GenerateToken("sadasdawggfa78f41a@$", "adsada dsa");
        Assert.IsTrue(Token.ValidateToken(token2, "sadasdawggfa78f41a@$", "adsada dsa"));

        var token3 = Token.GenerateToken("123", "321");
        Assert.IsFalse(Token.ValidateToken(token3, "321", "123"));
    }
}