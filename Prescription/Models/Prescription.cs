using System.ComponentModel.DataAnnotations;

namespace Prescription.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        [Required(ErrorMessage = "Medication name is required")]
        [StringLength(200, ErrorMessage = "Medication name is too long")]
        public string MedicationName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Fill status is required")]
        [RegularExpression("New|Filled|Pending", ErrorMessage = "Invalid fill status")]
        public string FillStatus { get; set; } = "New";

        [Required(ErrorMessage = "Cost is required")]
        [Range(0.1, double.MaxValue, ErrorMessage = "Cost must be greater than zero")]
        public double Cost { get; set; }

        [Required]
        public DateTime RequestTime { get; set; } = DateTime.Now;
        public string Slug => GenerateSlug(MedicationName);

        private static string GenerateSlug(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            var normalized = input.ToLowerInvariant().Trim();
            foreach (var c in new[] { ' ', '_', '/' }) normalized = normalized.Replace(c, '-');
            while (normalized.Contains("--")) normalized = normalized.Replace("--", "-");
            var sb = new System.Text.StringBuilder();
            foreach (var ch in normalized)
            {
                if (char.IsLetterOrDigit(ch) || ch == '-') sb.Append(ch);
            }
            return sb.ToString();
        }
    }
}
