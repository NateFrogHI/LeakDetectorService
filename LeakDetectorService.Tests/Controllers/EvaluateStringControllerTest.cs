using LeakDetectorService.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Moq;
using LeakDetectorService.Utils;
using LeakDetectorService.Models;

namespace LeakDetectorService.Tests.Controllers
{
    [TestClass]
    public class EvaluateStringControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            List<string> response = new List<string>();
            response.Add("Test Pass");
            Mock<IDb> mock = new Mock<IDb>();

            mock.Setup(library => library.GetLeakReports())
                .Returns(response);
            IDb db = mock.Object;
            EvaluateStringController controller = new EvaluateStringController(db);

            // Act

            List<string> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(response.ElementAt(0), result.ElementAt(0));
            mock.Verify(library => library.GetLeakReports(), Times.AtMostOnce());
        }

        [TestMethod]
        public void Post_where_multiple_violations_are_returned()
        {
            // Arrange
            List<string> restrictedStrings = new List<string>();
            restrictedStrings.Add("TeSt");
            restrictedStrings.Add("MULAT");
            Violation[] violations = { };
            LeakReport leakReport = new LeakReport(violations);
            RawText testString = new RawText() { Text = "This is test string for testing.\nIt simulates test stuff." };
            string reportStr = "Violations found:\n" +
                "violation: test\nLine Number: 1\nCharacter Number: 9\n" +
                "violation: test\nLine Number: 1\nCharacter Number: 25\n" +
                "violation: test\nLine Number: 2\nCharacter Number: 14\n" +
                "violation: mulat\nLine Number: 2\nCharacter Number: 6\n";

            Mock<IDb> mock = new Mock<IDb>();

            mock.Setup(library => library.GetRestrictedStrings())
                .Returns(restrictedStrings);
            mock.Setup(library => library.SubmitReport(leakReport))
                .Returns("Test Pass");
            IDb db = mock.Object;
            EvaluateStringController controller = new EvaluateStringController(db);

            // Act
            string result = controller.Post(testString);

            // Assert
            Assert.AreEqual(reportStr, result);
            mock.Verify(library => library.GetRestrictedStrings(), Times.AtMostOnce());
            mock.Verify(library => library.SubmitReport(leakReport), Times.AtMostOnce());
        }

        [TestMethod]
        public void Post_where_no_violations_are_returned()
        {
            // Arrange
            List<string> restrictedStrings = new List<string>();
            restrictedStrings.Add("supercalifragilisticexpialidocious");
            Violation[] violations = { };
            LeakReport leakReport = new LeakReport(violations);
            RawText testString = new RawText() { Text = "Not gonna find it." };
            string reportStr = "No violations found";

            Mock<IDb> mock = new Mock<IDb>();

            mock.Setup(library => library.GetRestrictedStrings())
                .Returns(restrictedStrings);
            mock.Setup(library => library.SubmitReport(leakReport))
                .Returns("Test Pass");
            IDb db = mock.Object;
            EvaluateStringController controller = new EvaluateStringController(db);

            // Act
            string result = controller.Post(testString);

            // Assert
            Assert.AreEqual(reportStr, result);
            mock.Verify(library => library.GetRestrictedStrings(), Times.AtMostOnce());
            mock.Verify(library => library.SubmitReport(leakReport), Times.AtMostOnce());
        }
    }
}
