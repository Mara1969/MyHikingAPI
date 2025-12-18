using FluentAssertions;
using MyHikingAPI.Models;
using MyHikingAPI.Services;


namespace MyHikingAPI.Tests
{
    public class JsonReaderTest
    {
        [Fact]
        public void GetData_CheckIfReturnsPopulatedListOfMountains()
        {
            //Arrange 
            String filename = "Data/mountainsTest.json";
                           
            //Act 
            List<Mountain> output = JsonReader.GetData<Mountain>(filename); 
            
            //Assert 
            output.Should().NotBeNullOrEmpty(); // checks if list is not null or empty 
            output.Count.Should().Be(2); // Check if the list contains exactly 2 items 
        }
    
        [Fact]
        public void GetData_WhenFileDoesNotExist_ShouldThrowFileNotFoundException()
        {
            // Arrange
            string filename = "Data/nonexistentfile.json";

            // Act
            Action act = () => JsonReader.GetData<Mountain>(filename);

            // Assert
            act.Should().Throw<FileNotFoundException>();
                //.WithMessage($"*{filename}*");
        }
    }
}
