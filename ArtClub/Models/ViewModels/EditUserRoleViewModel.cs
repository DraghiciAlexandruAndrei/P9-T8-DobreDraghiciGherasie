using ArtClub.Models.Enums;

namespace ArtClub.Models.ViewModels
{
	public class EditUserRoleViewModel
	{
		public int UserId { get; set; }
		public string UserName { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public UserRole CurrentRole { get; set; }
		public UserRole NewRole { get; set; }
	}
}