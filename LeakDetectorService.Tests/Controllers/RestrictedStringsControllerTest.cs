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
    public class RestrictedStringsControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            List<string> response = new List<string>();
            response.Add("test");
            Mock<IDb> mock = new Mock<IDb>();

            mock.Setup(library => library.GetRestrictedStrings())
                .Returns(response);
            IDb db = mock.Object;
            RestrictedStringsController controller = new RestrictedStringsController(db);

            // Act

            List<string> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(response.ElementAt(0), result.ElementAt(0));
            mock.Verify(library => library.GetRestrictedStrings(), Times.AtMostOnce());
        }

        [TestMethod]
        public void Put()
        {
            // Arrange
            string response = "1 record added to restrictedStrings";
            Restricted restrictedString = new Restricted() { RestrictedString = "newString" };
            Mock<IDb> mock = new Mock<IDb>();

            mock.Setup(library => library.AddRestrictedString(restrictedString))
                .Returns(response);
            IDb db = mock.Object;
            RestrictedStringsController controller = new RestrictedStringsController(db);

            // Act

            string result = controller.Put(restrictedString);

            // Assert
            Assert.AreEqual(response, result);
            mock.Verify(library => library.AddRestrictedString(restrictedString), Times.AtMostOnce());
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            string response = "Deleted 1 from restrictedStrings";
            Restricted restrictedString = new Restricted() { RestrictedString = "newString" };
            Mock<IDb> mock = new Mock<IDb>();

            mock.Setup(library => library.DeleteRestrictedString(restrictedString))
                .Returns(response);
            IDb db = mock.Object;
            RestrictedStringsController controller = new RestrictedStringsController(db);

            // Act

            string result = controller.Delete(restrictedString);

            // Assert
            Assert.AreEqual(response, result);
            mock.Verify(library => library.DeleteRestrictedString(restrictedString), Times.AtMostOnce());
        }
    }
}
