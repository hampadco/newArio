using System.Net.Http.Json;

public class UserService
{
    private readonly HttpClient _http;
    public UserService( HttpClient http)
    {
        _http=http;
    }

    public async Task<string> AddUser(User user)
{
    try
    {
        HttpResponseMessage response = await _http.PostAsJsonAsync("http://localhost:5298/User/AddUsers", user);

        if (response.IsSuccessStatusCode)
        {
            return "کاربر با موفقیت اضافه شد";
        }
        else
        {
            string errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"خطای سرور: {response.StatusCode} - {errorContent}");
            return $"خطا: {response.StatusCode} - {errorContent}";
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"استثناء رخ داد: {ex.Message}");
        return $"استثناء رخ داد: {ex.Message}";
    }
}


public async Task<bool> CheckLogin(LoginRequest loginData)
{
    try
    {
       
        // ارسال درخواست POST به سرور
        HttpResponseMessage response = await _http.PostAsJsonAsync("http://localhost:5298/User/CheckLogin", loginData);

        if (response.IsSuccessStatusCode)
        {
            // اگر پاسخ موفقیت‌آمیز بود، نتیجه true برمی‌گردد
            return true;
        }
        else
        {
            // اگر خطا رخ داد، خطای سرور را در کنسول نمایش می‌دهیم
            string errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"خطای سرور: {response.StatusCode} - {errorContent}");
            return false;
        }
    }
    catch (Exception ex)
    {
        // در صورت بروز استثناء، خطا را در کنسول چاپ کرده و false برمی‌گردانیم
        Console.WriteLine($"استثناء رخ داد: {ex.Message}");
        return false;
    }
}


}