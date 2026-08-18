using System.Diagnostics;
using System.Text.Json;

namespace VolunteerApp.Services {

    public class UserService {
        public static class UService {
            public static int? currentUserID;
            private const string ACTIVE_USER_KEY = "active_user_key";

            public static async Task<int?> GetCurrentUserIDAsync() {
                if (currentUserID == null) {
                    await LoadUserFromStorageAsync();
                }
                return currentUserID;
            }

            public static async Task SetCurrentUserIDAsync(int? userID) {
                SecureStorage.Remove(ACTIVE_USER_KEY);
                currentUserID = userID;

                string userJson = JsonSerializer.Serialize(userID);
                await SecureStorage.SetAsync(ACTIVE_USER_KEY, userJson);
            }

            public static void LogoutUser() {
                currentUserID = null;
                SecureStorage.Remove(ACTIVE_USER_KEY);
            }

            public static bool IsLoggedIn() {
                return currentUserID != null;
            }

            private static async Task LoadUserFromStorageAsync() {
                try {
                    string userJson = await SecureStorage.GetAsync(ACTIVE_USER_KEY) ?? "";
                    if (!string.IsNullOrEmpty(userJson)) {
                        currentUserID = JsonSerializer.Deserialize<int?>(userJson);
                    }
                }
                catch (Exception ex) {
                    Debug.WriteLine($"Error loading user: {ex.Message}");
                }
            }
        }

        public static class UserImgService {
            public static async Task<byte[]> FileResultToBytesAsync(FileResult file) {
                if (file == null)
                    return null;

                using var stream = await file.OpenReadAsync();
                using var memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }

            public static ImageSource BytesToImageSource(byte[] imageData) {
                if (imageData == null || imageData.Length == 0)
                    return null;

                return ImageSource.FromStream(() => new MemoryStream(imageData));
            }
        }
    }
}
