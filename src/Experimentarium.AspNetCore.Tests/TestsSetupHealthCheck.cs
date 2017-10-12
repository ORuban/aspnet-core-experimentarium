using System;
using Xunit;
using Experimentarium.AspNetCore.WebApi.Controllers;

namespace Experimentarium.AspNetCore.Tests
{
    // tests to check xUnit setup
    public class TestsSetupHealthCheck
    {
        [Fact]
         public void Green()
         {
            Assert.True(true);
         }

        [Fact]
         public void Red()
         {
            Assert.True(false);
         }

        [Fact(Skip="This test should be skipped")]
         public void Skipped()
         {
            Assert.True(false);
         }
    }
}
