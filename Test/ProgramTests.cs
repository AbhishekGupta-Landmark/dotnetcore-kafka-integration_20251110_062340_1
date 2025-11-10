{
  "testCasesFound": 0,
  "newTestCasesAdded": 3,
  "generatedTestCode": "using System;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore;

namespace Api.Tests
{
    public class ProgramTests
    {
        [Fact]
        public void CreateWebHostBuilder_ShouldNotBeNull()
        {
            // Arrange
            string[] args = new string[] { };

            // Act
            var webHostBuilder = Program.CreateWebHostBuilder(args);

            // Assert
            Assert.NotNull(webHostBuilder);
        }

        [Fact]
        public void CreateWebHostBuilder_ShouldUseStartup()
        {
            // Arrange
            string[] args = new string[] { };

            // Act
            var webHostBuilder = Program.CreateWebHostBuilder(args);

            // Assert
            Assert.Contains(typeof(Startup), webHostBuilder.GetType().GetProperties());
        }

        [Fact]
        public void Main_ShouldRunWebHost()
        {
            // Arrang...