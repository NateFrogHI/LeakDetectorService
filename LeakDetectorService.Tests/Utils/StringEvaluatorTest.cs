using LeakDetectorService.Utils;
using LeakDetectorService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace LeakDetectorService.Tests.Utils
{
    [TestClass]
    public class StringEvaluatorTest
    {
        [TestMethod]
        public void EvaluateString_with_violations()
        {
            // Arrange
            string testString = "This is test string.\nIt simulates stuff.";
            string[] restrictedStrings = { "TeSt", "MULAT" };
            Violation[] violations = {
                new Violation("test", 1, 9),
                new Violation("mulat", 2, 6),
            };

            // Act
            StringEvaluator stringEvaluator = new StringEvaluator(testString, restrictedStrings);
            stringEvaluator.EvaluateString();

            // Assert
            Assert.IsNotNull(stringEvaluator.Violations);
            Assert.AreEqual(2, stringEvaluator.Violations.Count());
            Assert.AreEqual(violations.ElementAt(0).ViolationString, stringEvaluator.Violations.ElementAt(0).ViolationString);
            Assert.AreEqual(violations.ElementAt(1).ViolationString, stringEvaluator.Violations.ElementAt(1).ViolationString);
            Assert.AreEqual(violations.ElementAt(0).LineNumber, stringEvaluator.Violations.ElementAt(0).LineNumber);
            Assert.AreEqual(violations.ElementAt(1).LineNumber, stringEvaluator.Violations.ElementAt(1).LineNumber);
            Assert.AreEqual(violations.ElementAt(0).CharacterNumber, stringEvaluator.Violations.ElementAt(0).CharacterNumber);
            Assert.AreEqual(violations.ElementAt(1).CharacterNumber, stringEvaluator.Violations.ElementAt(1).CharacterNumber);
        }

        [TestMethod]
        public void EvaluateString_without_violations()
        {
            // Arrange
            string testString = "This is test string.\nIt simulates stuff.";
            string[] restrictedStrings = { "supercalifragilisticexpialidocious" };

            // Act
            StringEvaluator stringEvaluator = new StringEvaluator(testString, restrictedStrings);
            stringEvaluator.EvaluateString();

            // Assert
            Assert.IsNull(stringEvaluator.Violations);
        }

        [TestMethod]
        public void HasViolations_with_violations()
        {
            // Arrange
            string testString = "";
            string[] restrictedStrings = {};
            Violation[] violations = {
                new Violation("test", 1, 9)
            };

            // Act
            StringEvaluator stringEvaluator = new StringEvaluator(testString, restrictedStrings);
            stringEvaluator.Violations = violations;
            bool result = stringEvaluator.HasViolations();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void HasViolations_without_violations()
        {
            // Arrange
            string testString = "";
            string[] restrictedStrings = { };

            // Act
            StringEvaluator stringEvaluator = new StringEvaluator(testString, restrictedStrings);
            bool result = stringEvaluator.HasViolations();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Report_with_violations()
        {
            // Arrange
            string testString = "";
            string[] restrictedStrings = {};
            Violation[] violations = {
                new Violation("test", 1, 9),
                new Violation("mulat", 2, 6),
            };
            string reportStr = "Violations found:\n" +
                "violation: test\nLine Number: 1\nCharacter Number: 9\n" +
                "violation: mulat\nLine Number: 2\nCharacter Number: 6\n";

            // Act
            StringEvaluator stringEvaluator = new StringEvaluator(testString, restrictedStrings);
            stringEvaluator.Violations = violations;
            string result = stringEvaluator.Report();

            // Assert
            Assert.AreEqual(reportStr, result);
        }

        [TestMethod]
        public void Report_without_violations()
        {
            // Arrange
            string testString = "";
            string[] restrictedStrings = { };
            string reportStr = "No violations found";

            // Act
            StringEvaluator stringEvaluator = new StringEvaluator(testString, restrictedStrings);
            string result = stringEvaluator.Report();

            // Assert
            Assert.AreEqual(reportStr, result);
        }
    }
}
