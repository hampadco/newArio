using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.JSInterop;

public class UserService
{
    private readonly HttpClient _http;
    private readonly IJSRuntime jsRuntime;
    private readonly string url = "http://localhost:5298";
    public UserService(HttpClient http, IJSRuntime _jsRuntime)
    {
        _http = http;
        jsRuntime = _jsRuntime;
    }

    public async Task DelToken()
    {
        await jsRuntime.InvokeVoidAsync("localStorage.removeItem", "jwtToken");
    }

    public async Task<string> CallToken()
    {
        string token = await jsRuntime.InvokeAsync<string>("localStorage.getItem", "jwtToken");
        return token;
    }

    private void SetToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization = null;
        _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
                await DelToken();
                return $"خطا: {response.StatusCode} - {errorContent}";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"استثناء رخ داد: {ex.Message}");
            await DelToken();
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
                var token = await response.Content.ReadFromJsonAsync<tokenJwt>();
                await jsRuntime.InvokeVoidAsync("localStorage.setItem", "jwtToken", token.Token);
                // اگر پاسخ موفقیت‌آمیز بود، نتیجه true برمی‌گردد
                return true;
            }
            else
            {
                // اگر خطا رخ داد، خطای سرور را در کنسول نمایش می‌دهیم
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"خطای سرور: {response.StatusCode} - {errorContent}");
                await DelToken();
                return false;
            }
        }
        catch (Exception ex)
        {
            // در صورت بروز استثناء، خطا را در کنسول چاپ کرده و false برمی‌گردانیم
            Console.WriteLine($"استثناء رخ داد: {ex.Message}");
            await DelToken();
            return false;
        }
    }

    public async Task<Traces> ViewTransactions(string token)
    {
        try
        {
            SetToken(token);
            HttpResponseMessage response = await _http.GetAsync($"{url}/Transaction/ViewTransactions");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<Traces>();
                return data ?? new Traces();
            }
            else
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"خطای سرور: {response.StatusCode} - {errorContent}");
                await DelToken();
                return new Traces() { Balance = 0, TraceHistories = [] };
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"استثناء رخ داد: {ex.Message}");
            await DelToken();
            return new Traces() { Balance = 0, TraceHistories = [] };
        }
    }

    public async Task<List<IncomingCard>> ShowCards(string token)
    {
        try
        {
            SetToken(token);
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
                await DelToken();
                return [];
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"استثناء رخ داد: {ex.Message}");
            await DelToken();
            return [];
        }
    }
    public async Task<bool> UpdateUser(User user, string token)
    {
        SetToken(token);
        HttpResponseMessage result = await _http.PutAsJsonAsync($"{url}/User/UpdateUser", user);
        if (result.IsSuccessStatusCode)
        {
            return true;
        }
        await DelToken();
        return false;
    }
    public async Task<User> getUser(string token)
    {
        SetToken(token);
        HttpResponseMessage result = await _http.GetAsync($"{url}/User/GetUser");
        if (result.IsSuccessStatusCode)
        {
            User? user = await result.Content.ReadFromJsonAsync<User>();
            return user!;
        }
        await DelToken();
        return new User();
    }
}