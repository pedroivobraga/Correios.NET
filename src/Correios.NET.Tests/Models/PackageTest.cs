﻿using Correios.NET.Exceptions;
using FluentAssertions;
using System;
using System.Globalization;
using Xunit;

namespace Correios.NET.Tests.Models
{
    public class PackageTest
    {
        private readonly string _packageHtml;
        private readonly string _packageDeliveredHtml;
        private readonly string _packageErrorHtml;
        private readonly string _packageCodeNotFound;

        public PackageTest()
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("pt-BR");
            _packageHtml = ResourcesReader.GetResourceAsString("Pacote.html");
            _packageDeliveredHtml = ResourcesReader.GetResourceAsString("PacoteEntregue.html");
            _packageErrorHtml = ResourcesReader.GetResourceAsString("PacoteNaoEncontrado.html");
            _packageCodeNotFound = ResourcesReader.GetResourceAsString("PacoteSemCodigo.html");
        }

        [Fact]
        public void PackageParse_ShouldBeReturnAValidPackage()
        {
            var package = Parser.ParsePackage(_packageHtml);
            package.Code.Should().NotBeNullOrEmpty();
            package.IsValid.Should().BeTrue();
            package.ShipDate.Should().Be(DateTime.Parse("13/09/2016 21:08:00"));
            package.IsDelivered.Should().BeFalse();
            package.DeliveryDate.HasValue.Should().BeFalse();
        }

        [Fact]
        public void PackageParse_ShouldBeReturnADeliveredPackage()
        {
            var package = Parser.ParsePackage(_packageDeliveredHtml);
            package.Code.Should().NotBeNullOrEmpty();
            package.IsValid.Should().BeTrue();
            package.IsDelivered.Should().BeTrue();
            package.ShipDate.Should().Be(DateTime.Parse("12/09/2016 14:57:00"));
            package.DeliveryDate.Should().Be(DateTime.Parse("13/09/2016 18:42:00"));
        }

        [Fact]
        public void PackageParse_ShouldThrowAnParseException()
        {
            Action act = () => Parser.ParsePackage(_packageErrorHtml);
            act.Should().Throw<ParseException>();
        }

        [Fact]
        public void PackageParse_ShouldThrowAnParseExceptionCodeNotFound()
        {
            Action act = () => Parser.ParsePackage(_packageCodeNotFound);
            act.Should().Throw<ParseException>().WithMessage("Código da encomenda/pacote não foi encontrado.");
        }
    }
}
