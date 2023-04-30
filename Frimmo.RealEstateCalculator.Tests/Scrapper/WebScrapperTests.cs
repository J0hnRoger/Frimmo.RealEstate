using System.Threading.Tasks;
using FluentAssertions;
using Frimmo.Scrapper;
using Xunit;

namespace Frimmo.RealEstateCalculator.Tests.Scrapper;

public class WebScrapperTests
{
   [Fact]
   public async Task Scrapper_ReturnLeBonCoinPageContent()
   {
      // TODO - LBC a mis en place une sécurité de type Captcha - un puzzle à résoudre. 
      // Donc pour recupérer les offres, il faut une solution browser-based type Cypress
      string adUrl = "https://www.leboncoin.fr/ventes_immobilieres/2289350532.htm";
      // adUrl = "https://www.leboncoin.fr/ventes_immobilieres/offres/rhone_alpes/rhone/";
      // adUrl = "https://www.leboncoin.fr/f/ventes_immobilieres/real_estate_type--2?locations=Les%2520Herbiers"; 
      // adUrl = "https://www.leboncoin.fr/recherche?category=9&locations=Les%20Herbiers"; 
      var sut = new WebScrapper();
      var result = await sut.GetContent(adUrl);
      result.Should().NotBeNull();
   }
}