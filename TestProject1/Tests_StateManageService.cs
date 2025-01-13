using Microsoft.Extensions.Options;
using Microsoft.Extensions.Time.Testing;
using OAuthExample.Service.Options;
using OAuthExample.Service.Services;

namespace TestProject1
{
    public class Tests_StateManageService
    {
        private StateManageService _stateManageService;
        private FakeTimeProvider _timeProvider;
        private int _validityMinutes;
        private DateTimeOffset _baseTime;
        private string _encryptionKey;

        private StateManageService CreateStateManageService(string encryptionKey)
        {
            var options = Options.Create(new StateManageOptions
            {
                ValidityMinutes = _validityMinutes,
                EncryptionKey = encryptionKey
            });
            return new StateManageService(options, _timeProvider);
        }

        [SetUp]
        public void Setup()
        {
            _encryptionKey = "0123456789ABCDEF";
            _validityMinutes = 10;
            _baseTime = new DateTimeOffset(2024, 1, 1, 0, 0, 0, TimeSpan.Zero);
            _timeProvider = new FakeTimeProvider(_baseTime);
            _stateManageService = CreateStateManageService(_encryptionKey);
        }

        [Test]
        public void Test_Empty_State()
        {
            // Act
            string state = string.Empty;
            bool isValid = _stateManageService.ValidateState(state);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void Test_Vaild_State()
        {
            // Act
            string state = _stateManageService.GenerateState();
            bool isValid = _stateManageService.ValidateState(state);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Test_Expired_State_After_Validity()
        {
            // Act
            string state = _stateManageService.GenerateState();
            _timeProvider.SetUtcNow(_baseTime.AddMinutes(_validityMinutes));
            bool isValid = _stateManageService.ValidateState(state);

            // Assert
            Assert.IsTrue(isValid);
        }

        [Test]
        public void Test_Invalid_State()
        {
            // Act
            string state = "InvalidState";
            bool isValid = _stateManageService.ValidateState(state);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void Test_Expired_State()
        {
            // Act
            string state = _stateManageService.GenerateState();
            _timeProvider.SetUtcNow(_baseTime.AddMinutes(_validityMinutes).AddSeconds(1));
            bool isValid = _stateManageService.ValidateState(state);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void Test_Invalid_EncryptionKey()
        {
            // Arrange
            var invalidKeyStateManageService = CreateStateManageService("0123456789ABCDEE");

            // Act
            string state = invalidKeyStateManageService.GenerateState();
            bool isValid = _stateManageService.ValidateState(state);

            // Assert
            Assert.IsFalse(isValid);
        }

        [Test]
        public void Test_Valid_EncryptionKey()
        {
            // Arrange
            var validKeyStateManageService = CreateStateManageService(_encryptionKey);

            // Act
            string state = validKeyStateManageService.GenerateState();
            bool isValid = _stateManageService.ValidateState(state);

            // Assert
            Assert.IsTrue(isValid);
        }
    }
}