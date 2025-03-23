[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace StegaByte.Tests
{
    [Collection("Sequential")]
    public class EncoderTests
    {
        [Fact]
        public void Encode_StringText()
        {
            // Declare a string type variable with an assigned value (A simple Lorem ipsum in this case)
            string stringText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa.";
            // Declare the path string variable containing where the image will be saved and it's name (Temp folder and StegaByteStringText.png in this case).
            string uniqueFileName = $"StegaByte_{Guid.NewGuid()}.png";
            string filePath = Path.Combine(Path.GetTempPath(), uniqueFileName);
            try
            {
                // Encode the desired data onto the image at the specified path using the Encoder class.
                Encoder.Encode(stringText, filePath);
                // Read the encoded data and explicitly cast it to a string type using the Decoder class.
                string readStringText = (string)Decoder.Decode(filePath);
                // Check if the decoded data equals the original data. If so the test passes.
                Assert.Equal(stringText, readStringText);
            }
            finally
            {
                // Cleanup
                if (File.Exists(filePath)) File.Delete(filePath);
            }
        }
        [Fact]
        public void Encode_ClassInstance()
        {
            // Declare and assign a Person type class instance.
            Person classInstance = new Person("Dave Corall", 47, "Yellow");
            // Declare the path string variable containing where the image will be saved and it's name (Temp folder and StegaByteClassInstance.png in this case).
            string uniqueFileName = $"StegaByte_{Guid.NewGuid()}.png";
            string filePath = Path.Combine(Path.GetTempPath(), uniqueFileName);
            try
            {
                // Encode the desired data onto the image at the specified path using the Encoder class.
                Encoder.Encode(classInstance, uniqueFileName);
                // Read the encoded data and explicitly cast it to a string type using the Decoder class.
                Person readClassInstance = (Person)Decoder.Decode(uniqueFileName);
                // Check if the decoded data equals the original data. If so the test passes.
                Assert.Equal(classInstance.Name, readClassInstance.Name);
                Assert.Equal(classInstance.Age, readClassInstance.Age);
                Assert.Equal(classInstance.FavoriteColor, readClassInstance.FavoriteColor);
            }
            finally
            {
                // Cleanup
                if (File.Exists(uniqueFileName)) File.Delete(uniqueFileName);
            }
        }
        [Fact]
        public void Encode_ClassList()
        {
            // Declare and assign a Person type List variable.
            List<Person> classList = new()
            {
                new Person("John Doe", 23, "Blue"),
                new Person("Jane Doe", 25, "Red"),
                new Person("Joe Bloggs", 31, "Purple")
            };
            // Declare the path string variable containing where the image will be saved and it's name (Temp folder and StegaByteStringText.png in this case).
            string uniqueFileName = $"StegaByte_{Guid.NewGuid()}.png";
            string filePath = Path.Combine(Path.GetTempPath(), uniqueFileName);
            try
            {
                // Encode the desired data onto the image at the specified path using the Encoder class.
                Encoder.Encode(classList, uniqueFileName);
                // Read the encoded data and explicitly cast it to a string type using the Decoder class.
                List<Person> readClassList = (List<Person>)Decoder.Decode(uniqueFileName);

                // Check if the decoded data equals the original data. If so the test passes.
                Assert.Equal(classList[0].Name, readClassList[0].Name);
                Assert.Equal(classList[0].Age, readClassList[0].Age);
                Assert.Equal(classList[0].FavoriteColor, readClassList[0].FavoriteColor);

                Assert.Equal(classList[1].Name, readClassList[1].Name);
                Assert.Equal(classList[1].Age, readClassList[1].Age);
                Assert.Equal(classList[1].FavoriteColor, readClassList[1].FavoriteColor);

                Assert.Equal(classList[2].Name, readClassList[2].Name);
                Assert.Equal(classList[2].Age, readClassList[2].Age);
                Assert.Equal(classList[2].FavoriteColor, readClassList[2].FavoriteColor);
            }
            finally
            {
                // Cleanup
                if (File.Exists(uniqueFileName)) File.Delete(uniqueFileName);
            }
        }
        [Fact]
        public void Encode_ComplexClass()
        {
            // Declare a ComplexClass type variable with an assigned value.
            var complexClass = new ComplexClass("John Doe", 23, "Blue");
            // Declare the path string variable containing where the image will be saved and it's name (Temp folder and StegaByteStringTextImagePath.png in this case).
            string uniqueFileName = $"StegaByte_{Guid.NewGuid()}.png";
            string filePath = Path.Combine(Path.GetTempPath(), uniqueFileName);
            try
            {
                // Encode the desired data onto the image at the specified path using the Encoder class.
                Encoder.Encode(complexClass, filePath);
                // Read the encoded data and explicitly cast it to a string type using the Decoder class.
                ComplexClass readComplexClass = (ComplexClass)Decoder.Decode(filePath);
                // Check if the decoded data equals the original data. If so the test passes.
                Assert.Equal(complexClass.someText, readComplexClass.someText);
                Assert.Equal(complexClass.someNumber, readComplexClass.someNumber);
                Assert.Equal(complexClass.anotherNumber, readComplexClass.anotherNumber);

                Assert.Equal(410 + 1423, readComplexClass.DoSomething());
            }
            finally
            {
                // Cleanup
                if (File.Exists(filePath)) File.Delete(filePath);
            }
        }
        [Fact]
        public void Encode_NullObject_ThrowsArgumentNullException()
        {
            string uniqueFileName = $"StegaByte_{Guid.NewGuid()}.png";
            string filePath = Path.Combine(Path.GetTempPath(), uniqueFileName); try
            {
                var exception = Assert.Throws<ArgumentNullException>(() => Encoder.Encode(null, filePath));
                Assert.Equal("obj", exception.ParamName);
            }
            finally
            {
                // Cleanup
                if (File.Exists(filePath)) File.Delete(filePath);
            }
        }
        [Fact]
        public void Encode_EmptyString_CreatesPngFile()
        {
            string testObject = string.Empty;
            string uniqueFileName = $"StegaByte_{Guid.NewGuid()}.png";
            string filePath = Path.Combine(Path.GetTempPath(), uniqueFileName); try
            {
                Encoder.Encode(testObject, filePath);
            }
            finally
            {
                // Cleanup
                if (File.Exists(filePath)) File.Delete(filePath);
            }
        }
        /// <summary>
        /// Simple dummy class to test StegaByte's things on.
        /// </summary>
        internal class Person
        {
            public string Name { get; private set; } = string.Empty;
            public int Age { get; private set; }
            public string FavoriteColor { get; private set; } = string.Empty;
            public Person(string name, int age, string favoriteColor)
            {
                Name = name;
                Age = age;
                FavoriteColor = favoriteColor;
            }
        }
        /// <summary>
        /// Complex dummy class to test StegaByte's things on.
        /// </summary>
        internal class ComplexClass
        {
            public string someText = "Lorem ipsum dolor sit amet, consectetuer adipiscing elit. Aenean commodo ligula eget dolor. Aenean massa.";
            public int someNumber = 410;
            public int anotherNumber = 1423;
            public Person? nestedClass { get; set; }
            public ComplexClass(string name, int age, string favoriteColor)
            {
                nestedClass = new(name, age, favoriteColor);
            }
            public ComplexClass()
            {
            }
            public int DoSomething()
            {
                return someNumber + anotherNumber;
            }
        }
    }
}