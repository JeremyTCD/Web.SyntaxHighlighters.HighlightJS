﻿using Microsoft.AspNetCore.NodeServices;
using Microsoft.AspNetCore.NodeServices.HostingModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace JeremyTCD.WebUtils.SyntaxHighlighters.HighlightJS.Tests
{
    public class HighlightJSServiceUnitTests
    {
        private readonly MockRepository _mockRepository = new MockRepository(MockBehavior.Default) { DefaultValue = DefaultValue.Mock };

        //[Fact]
        //public async Task Highlight_ThrowsExceptionIfCodeIsNull()
        //{
        //    // Arrange
        //    HighlightJSService prismService = CreatePrismService();

        //    // Act and assert
        //    ArgumentNullException result = await Assert.ThrowsAsync<ArgumentNullException>(() => prismService.HighlightAsync(null, null)).ConfigureAwait(false);
        //}

        //[Theory]
        //[MemberData(nameof(Highlight_ReturnsCodeIfCodeIsEmptyOrWhitespace_Data))]
        //public async Task Highlight_ReturnsCodeIfCodeIsEmptyOrWhitespace(string dummyCode)
        //{
        //    // Arrange
        //    HighlightJSService prismService = CreatePrismService();

        //    // Act
        //    string result = await prismService.HighlightAsync(dummyCode, null).ConfigureAwait(false);

        //    // Assert
        //    Assert.Equal(dummyCode, result);
        //}

        //public static IEnumerable<object[]> Highlight_ReturnsCodeIfCodeIsEmptyOrWhitespace_Data()
        //{
        //    return new object[][]
        //    {
        //        new object[]
        //        {
        //            string.Empty
        //        },
        //        new object[]
        //        {
        //            " "
        //        }
        //    };
        //}

        //[Fact]
        //public async Task Highlight_ThrowsExceptionIfLanguageAliasIsNotAValidPrismLanguageAlias()
        //{
        //    // Arrange
        //    const string dummyCode = "dummyCode";
        //    const string dummyLanguageAlias = "dummyLanguageAlias";
        //    Mock<HighlightJSService> mockPrismService = CreateMockPrismService();
        //    mockPrismService.CallBase = true;
        //    mockPrismService.Setup(p => p.IsValidLanguageAliasAsync(dummyLanguageAlias)).ReturnsAsync(false);

        //    // Act and assert
        //    ArgumentException result = await Assert.ThrowsAsync<ArgumentException>(() => mockPrismService.Object.HighlightAsync(dummyCode, dummyLanguageAlias)).ConfigureAwait(false);
        //    Assert.Equal(result.Message, string.Format(Strings.Exception_InvalidPrismLanguageAlias, dummyLanguageAlias));
        //    _mockRepository.VerifyAll();
        //}

        //[Fact]
        //public async Task Highlight_ThrowsExceptionIfANodeErrorOccurs()
        //{
        //    // Arrange
        //    const string dummyCode = "dummyCode";
        //    const string dummyLanguageAlias = "dummyLanguageAlias";
        //    var dummyNodeInvocationException = new NodeInvocationException("", "");
        //    var dummyAggregateException = new AggregateException("", dummyNodeInvocationException);
        //    Mock<INodeServices> mockNodeServices = _mockRepository.Create<INodeServices>();
        //    mockNodeServices.Setup(n => n.InvokeExportAsync<string>(HighlightJSService.BUNDLE, "highlight", dummyCode, dummyLanguageAlias)).ThrowsAsync(dummyAggregateException);
        //    Mock<HighlightJSService> mockPrismService = CreateMockPrismService(mockNodeServices.Object);
        //    mockPrismService.CallBase = true;
        //    mockPrismService.Setup(p => p.IsValidLanguageAliasAsync(dummyLanguageAlias)).ReturnsAsync(true);

        //    // Act and assert
        //    NodeInvocationException result = await Assert.ThrowsAsync<NodeInvocationException>(() => mockPrismService.Object.HighlightAsync(dummyCode, dummyLanguageAlias)).ConfigureAwait(false);
        //    Assert.Same(dummyNodeInvocationException, result);
        //    _mockRepository.VerifyAll();
        //}

        //[Fact]
        //public async Task Highlight_IfSuccessfulInvokesHighlightInInteropJSAndReturnsHighlightedCode()
        //{
        //    // Arrange
        //    const string dummyCode = "dummyCode";
        //    const string dummyHighlightedCode = "dummyHighlightedCode";
        //    const string dummyLanguageAlias = "dummyLanguageAlias";
        //    Mock<INodeServices> mockNodeServices = _mockRepository.Create<INodeServices>();
        //    mockNodeServices.Setup(n => n.InvokeExportAsync<string>(HighlightJSService.BUNDLE, "highlight", dummyCode, dummyLanguageAlias)).ReturnsAsync(dummyHighlightedCode);
        //    Mock<HighlightJSService> mockPrismService = CreateMockPrismService(mockNodeServices.Object);
        //    mockPrismService.CallBase = true;
        //    mockPrismService.Setup(p => p.IsValidLanguageAliasAsync(dummyLanguageAlias)).ReturnsAsync(true);

        //    // Act
        //    string result = await mockPrismService.Object.HighlightAsync(dummyCode, dummyLanguageAlias).ConfigureAwait(false);

        //    // Assert
        //    Assert.Equal(dummyHighlightedCode, result);
        //    _mockRepository.VerifyAll();
        //}

        [Theory]
        [MemberData(nameof(IsValidLanguageAlias_ReturnsFalseIfLanguageAliasIsNullOrWhitespace_Data))]
        public async Task IsValidLanguageAlias_ReturnsFalseIfLanguageAliasIsNullOrWhitespace(string dummyLanguageAlias)
        {
            // Arrange
            HighlightJSService prismService = CreatePrismService();

            // Act
            bool result = await prismService.IsValidLanguageNameOrAliasAsync(dummyLanguageAlias).ConfigureAwait(false);

            // Assert
            Assert.False(result);
        }

        public static IEnumerable<object[]> IsValidLanguageAlias_ReturnsFalseIfLanguageAliasIsNullOrWhitespace_Data()
        {
            return new object[][]
            {
                new object[]
                {
                    null
                },
                new object[]
                {
                    string.Empty
                },
                new object[]
                {
                    " "
                }
            };
        }

        [Theory]
        [MemberData(nameof(IsValidLanguageAlias_IfSuccessfulReturnsTrueIfAliasesContainsLanguageAliasAndFalseIfItDoesNot_Data))]
        public async Task IsValidLanguageAlias_IfSuccessfulReturnsTrueIfAliasesContainsLanguageAliasAndFalseIfItDoesNot(
            string dummyLanguageAlias,
            string[] dummyAliases,
            bool expectedResult)
        {
            // Arrange
            Mock<INodeServices> mockNodeServices = _mockRepository.Create<INodeServices>();
            mockNodeServices.Setup(n => n.InvokeExportAsync<string[]>(HighlightJSService.BUNDLE, "getAliases")).ReturnsAsync(dummyAliases);
            HighlightJSService prismService = CreatePrismService(mockNodeServices.Object);

            // Act
            bool result = await prismService.IsValidLanguageNameOrAliasAsync(dummyLanguageAlias).ConfigureAwait(false);

            // Assert
            Assert.Equal(expectedResult, result);
            _mockRepository.VerifyAll();
        }

        public static IEnumerable<object[]> IsValidLanguageAlias_IfSuccessfulReturnsTrueIfAliasesContainsLanguageAliasAndFalseIfItDoesNot_Data()
        {
            const string dummyLanguageAlias = "dummyLanguageAlias";

            return new object[][]
            {
                // If aliases contains language alias, should return true
                new object[]
                {
                    dummyLanguageAlias,
                    new string[]{ dummyLanguageAlias },
                    true
                },
                // Otherwise, should return false
                new object[]
                {
                    dummyLanguageAlias,
                    new string[0],
                    false
                }
            };
        }

        [Fact]
        public async Task IsValidLanguageAlias_ThrowsExceptionIfANodeErrorOccurs()
        {
            // Arrange
            const string dummyLanguageAlias = "dummyLanguageAlias";
            var dummyNodeInvocationException = new NodeInvocationException("", "");
            var dummyAggregateException = new AggregateException("", dummyNodeInvocationException);
            Mock<INodeServices> mockNodeServices = _mockRepository.Create<INodeServices>();
            mockNodeServices.Setup(n => n.InvokeExportAsync<string[]>(HighlightJSService.BUNDLE, "getAliases")).ThrowsAsync(dummyAggregateException);
            HighlightJSService prismService = CreatePrismService(mockNodeServices.Object);

            // Act and assert
            NodeInvocationException result = await Assert.ThrowsAsync<NodeInvocationException>(() => prismService.IsValidLanguageNameOrAliasAsync(dummyLanguageAlias)).ConfigureAwait(false);
            Assert.Same(dummyNodeInvocationException, result);
            _mockRepository.VerifyAll();
        }

        private HighlightJSService CreatePrismService(INodeServices nodeServices = null)
        {
            return new HighlightJSService(nodeServices);
        }

        private Mock<HighlightJSService> CreateMockPrismService(INodeServices nodeServices = null)
        {
            return _mockRepository.Create<HighlightJSService>(nodeServices);
        }
    }
}
