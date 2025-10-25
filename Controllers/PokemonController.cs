using Newtonsoft.Json.Linq;
using PokeApiMvc.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PokeApiMvc.Controllers
{
    public class PokemonController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(string pokemonName)
        {
            if (string.IsNullOrEmpty(pokemonName))
            {
                ViewBag.Error = "Por favor, ingresa un nombre de Pokémon.";
                return View();
            }

            string apiUrl = $"https://pokeapi.co/api/v2/pokemon/{pokemonName.ToLower()}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string json = await response.Content.ReadAsStringAsync();
                        JObject data = JObject.Parse(json);

                        var pokemon = new Pokemon
                        {
                            Name = data["name"]?.ToString(),
                            Height = (double)data["height"] / 10,
                            Weight = (double)data["weight"] / 10,
                            ImageUrl = data["sprites"]?["front_default"]?.ToString(),
                            Types = new List<string>()
                        };

                        foreach (var type in data["types"])
                        {
                            pokemon.Types.Add(type["type"]?["name"]?.ToString());
                        }

                        return View(pokemon);
                    }
                    else
                    {
                        ViewBag.Error = $"No se encontró el Pokémon '{pokemonName}'.";
                        return View();
                    }
                }
                catch (System.Exception ex)
                {
                    ViewBag.Error = $"Error al conectar con la API: {ex.Message}";
                    return View();
                }
            }
        }
    }
}
