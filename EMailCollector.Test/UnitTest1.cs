using EMailCollector.POCApp;

namespace EMailCollector.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var text = "this is some Email: <a href=\"mailto:someemail@somemail.com\">send email</a> text which contains some another@someemail.lu email addresses inside";

            List<string> emailsFound = Helpers.ExtractEmails(text);

            Assert.Equal(2, emailsFound.Count);
            Assert.Equal("someemail@somemail.com", emailsFound[0]);
            Assert.Equal("another@someemail.lu", emailsFound[1]);
        }
    }
}