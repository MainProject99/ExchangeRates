using AutoMapper;
using DataAccessLayer.IRepositories;
using Moq;
using NUnit.Framework;

namespace BusinessLogicLayer.Test
{
    [TestFixture]
    public abstract class TestInitializer
    {
        protected static Mock<IMapper> mockMapper;

        [SetUp]
        protected virtual void Initialize()
        {
            mockMapper = new Mock<IMapper>();
            TestContext.WriteLine("Initialize test data");
        }

        [TearDown]
        protected virtual void Cleanup()
        {
            TestContext.WriteLine("Cleanup test data");
        }

    }
}