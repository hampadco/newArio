using System.Net.Http.Json;

public class UserService
{
    private readonly HttpClient _http;
    private readonly string url = "http://localhost:5298";
    public UserService(HttpClient http)
    {
        _http = http;
    }

    public async Task<string> AddUser(User user)
    {
        try
        {
            HttpResponseMessage response = await _http.PostAsJsonAsync($"{url}/User/AddUsers", user);

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
            HttpResponseMessage response = await _http.PostAsJsonAsync($"{url}/User/CheckLogin", loginData);

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

    public async Task<Traces> ViewTransactions(int UserId)
    {
        try
        {
            HttpResponseMessage response = await _http.GetAsync($"{url}/Transaction/ViewTransactions?UserId={UserId}");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Traces>();
                return data ?? new Traces();
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"خطای سرور: {response.StatusCode} - {errorContent}");
                return new Traces() { Balance = 0, TraceHistories = [] };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"استثناء رخ داد: {ex.Message}");
            return new Traces() { Balance = 0, TraceHistories = [] };
        }
    }

    public async Task<List<IncomingCard>> ShowCards()
    {
        try
        {
            HttpResponseMessage response = await _http.GetAsync($"{url}/Card/ShowCards");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<List<IncomingCard>>();
                return data ?? [];
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"خطای سرور: {response.StatusCode} - {errorContent}");
                return [];
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"استثناء رخ داد: {ex.Message}");
            return [];
        }
    }
}