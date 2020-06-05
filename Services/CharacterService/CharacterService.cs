using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using dotnet_rpg.Data;
using dotnet_rpg.Dtos.Character;
using dotnet_rpg.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Services.CharacterService
{
	public class CharacterService : ICharacterService
	{

		// private static List<Character> characters = new List<Character>
		// {
		// 	new Character(),
		// 	new Character{Id = 1, Name = "Sam"}
		// };

		private readonly IMapper _mapper;
		private readonly DataContext _context;

		public CharacterService(IMapper mapper, DataContext context)
		{
			_mapper = mapper;
			_context = context;
		}

		public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
		{
			ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
			Character character = _mapper.Map<Character>(newCharacter);
			// character.Id = characters.Count;
			// characters.Add(character);

			await _context.Characters.AddAsync(character);
			await _context.SaveChangesAsync();
			serviceResponse.Data = (_context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
			return serviceResponse;
		}

		public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
		{
			ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

			List<Character> dbCharacters = await _context.Characters.ToListAsync();
			// serviceResponse.Data = (characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
			serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
		{
			ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
			Character dbCharacters = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
			// serviceResponse.Data = _mapper.Map<GetCharacterDto>(characters.FirstOrDefault(c => c.Id == id));
			serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacters);
			return serviceResponse;
		}

		public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
		{
			ServiceResponse<GetCharacterDto> serviceResponse = new ServiceResponse<GetCharacterDto>();
			try
			{
				// Character character = characters.FirstOrDefault(c => c.Id == updateCharacter.Id);
				Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);
				character.Name = updateCharacter.Name;
				character.Class = updateCharacter.Class;
				character.Defence = updateCharacter.Defence;
				character.HitPoints = updateCharacter.HitPoints;
				character.Intelligence = updateCharacter.Intelligence;
				character.Strength = updateCharacter.Strength;

				_context.Characters.Update(character);
				await _context.SaveChangesAsync();
				serviceResponse.Data = _mapper.Map<GetCharacterDto>(character);
				serviceResponse.Message = "Character Updated Successfully";
			}
			catch (Exception ex)
			{
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}
			return serviceResponse;
		}


		public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
		{
			ServiceResponse<List<GetCharacterDto>> serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
			try
			{
				// Character character = characters.First(c => c.Id == id);
				// characters.Remove(character);

				Character character = await _context.Characters.FirstAsync(c => c.Id == id);

				_context.Characters.Remove(character);
				await _context.SaveChangesAsync();

				serviceResponse.Data = (_context.Characters.Select(c => _mapper.Map<GetCharacterDto>(c))).ToList();
				serviceResponse.Message = "Character Deleted Successfully";
			}
			catch (Exception ex)
			{
				serviceResponse.Success = false;
				serviceResponse.Message = ex.Message;
			}
			return serviceResponse;
		}

	}
}