using SGC.Domain.Interfaces;
using StackExchange.Redis;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SGC.Infrastructure.Cache
{
    // Implementación concreta del servicio de caché utilizando Redis
    public class RedisCacheService : ICacheService
    {
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        // Recupera un valor del caché por su clave
        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNullOrEmpty) return default;
            return JsonSerializer.Deserialize<T>(value!);
        }

        // Almacena un valor en el caché con una clave y una expiración opcional
        public async Task SetAsync<T>(string key, T value,
            TimeSpan? expiration = null)
        {
            var json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, expiration);
        }

        // Elimina un valor del caché por su clave
        public async Task RemoveAsync(string key)
        {
            await _db.KeyDeleteAsync(key);
        }

        // Verifica si una clave existe en el caché
        public async Task<bool> ExistsAsync(string key)
        {
            return await _db.KeyExistsAsync(key);
        }
    }
}