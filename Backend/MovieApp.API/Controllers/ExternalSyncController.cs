using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MovieApp.Business.Abstract;
using MovieApp.DataAccess.Abstract;
using MovieApp.DTO.DTOs.ExternalDtos;
using MovieApp.DTO.DTOs.MovieDtos;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MovieApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExternalSyncController : ControllerBase
    {
        private readonly IExternalMovieService _external;
        private readonly IExternalPersonService _externalPerson;
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;
        private readonly IActorService _actorService;
        private readonly IDirectorService _directorService;
        private readonly ILanguageService _languageService;
        private readonly IMapper _mapper;
        private readonly string _tmdbApiKey;

        public ExternalSyncController(
            IExternalMovieService external,
            IExternalPersonService externalPersonService,
            IMovieService movieService,
            IGenreService genreService,
            IActorService actorService,
            IDirectorService directorService,
            ILanguageService languageService,
            IMapper mapper,
            IConfiguration config)
        {
            _external = external;
            _externalPerson = externalPersonService;
            _movieService = movieService;
            _genreService = genreService;
            _actorService = actorService;
            _directorService = directorService;
            _languageService = languageService;
            _mapper = mapper;
            _tmdbApiKey = config["Tmdb:ApiKey"];
        }

        [HttpPost("movie/{tmdbId}/sync")]
        public async Task<IActionResult> SyncMovie(int tmdbId)
        {
            var details = await _external.GetFullDetailsAsync(_tmdbApiKey, tmdbId);

            var dto = _mapper.Map<CreateMovieDto>(details);
            dto.ExternalId = details.id;

            dto.SpokenLanguageCodes.Clear();
            foreach (var sl in details.spoken_languages)
            {
                var lang = await _languageService.GetByIso639Async(sl.iso_639_1);
                if (lang != null)
                    dto.SpokenLanguageCodes.Add(lang.Iso639);
                else
                {
                    var createdLang = await _languageService.CreateFromExternalAsync(
                        new TmdbLanguageDto
                        {
                            iso_639_1 = sl.iso_639_1,
                            english_name = sl.english_name,
                            name = sl.name
                        });
                    dto.SpokenLanguageCodes.Add(createdLang.Iso639);
                }
            }

            dto.GenreIds.Clear();
            foreach (var g in details.genres)
            {
                var ex = await _genreService.GetByExternalIdAsync(g.id);
                if (ex != null)
                    dto.GenreIds.Add(ex.Id);
                else
                {
                    var ct = await _genreService.CreateFromExternalAsync(
                        new TmdbGenreDto { id = g.id, name = g.name });
                    dto.GenreIds.Add(ct.Id);
                }
            }

            dto.Cast.Clear();
            foreach (var c in details.credits.cast
                                 .OrderBy(c => c.order)
                                 .Take(10))
            {
                var actor = await _actorService.GetByExternalIdAsync(c.id)
                            ?? await _actorService.CreateFromExternalAsync(
                                new TmdbCastDto
                                {
                                    id = c.id,
                                    name = c.name,
                                    character = c.character,
                                    profile_path = c.profile_path,
                                    popularity = c.popularity
                                });

                var pers = await _externalPerson.GetPersonAsync(_tmdbApiKey, c.id);
                actor.Biography = pers.biography;
                actor.Gender = (byte)pers.gender;
                actor.Popularity = (float)pers.popularity;
                if (DateTime.TryParseExact(
                        pers.birthday,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var bd))
                    actor.Birthday = bd;
                if (!string.IsNullOrWhiteSpace(pers.place_of_birth))
                    actor.PlaceOfBirth = pers.place_of_birth;

                await _actorService.UpdateAsync(actor);

                dto.Cast.Add(new ActorCastDto
                {
                    ActorId = actor.Id,
                    Character = c.character ?? string.Empty,
                    Order = (short)c.order
                });
            }

            dto.DirectorIds.Clear();
            foreach (var cr in details.credits.crew.Where(x => x.job == "Director"))
            {
                var dir = await _directorService.GetByExternalIdAsync(cr.id)
                          ?? await _directorService.CreateFromExternalAsync(
                              new TmdbCrewDto
                              {
                                  id = cr.id,
                                  name = cr.name,
                                  job = cr.job,
                                  department = cr.department,
                                  profile_path = cr.profile_path,
                                  popularity = cr.popularity
                              });

                var pers = await _externalPerson.GetPersonAsync(_tmdbApiKey, cr.id);
                dir.Biography = pers.biography;
                if (DateTime.TryParseExact(
                        pers.birthday,
                        "yyyy-MM-dd",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out var bd2))
                    dir.Birthday = bd2;
                if (!string.IsNullOrWhiteSpace(pers.place_of_birth))
                    dir.PlaceOfBirth = pers.place_of_birth;
                dir.Popularity = (float)pers.popularity;

                await _directorService.UpdateAsync(dir);
                dto.DirectorIds.Add(dir.Id);
            }

            dto.ImageMetas.Clear();
            foreach (var img in details.images.posters)
            {
                dto.ImageMetas.Add(new ImageMetaDto
                {
                    Type = "poster",
                    FilePath = img.file_path,
                    Width = (short)img.width,
                    Height = (short)img.height,
                    VoteAverage = (float)img.vote_average
                });
            }
            foreach (var img in details.images.backdrops)
            {
                dto.ImageMetas.Add(new ImageMetaDto
                {
                    Type = "backdrop",
                    FilePath = img.file_path,
                    Width = (short)img.width,
                    Height = (short)img.height,
                    VoteAverage = (float)img.vote_average
                });
            }

            var movie = await _movieService.UpsertFromExternalAsync(dto);

            return Ok(new
            {
                synced = tmdbId,
                movieId = movie.Id,
                posterCount = details.images.posters.Count,
                backdropCount = details.images.backdrops.Count,
                imageMetas = dto.ImageMetas
            });
        }
    }
}
