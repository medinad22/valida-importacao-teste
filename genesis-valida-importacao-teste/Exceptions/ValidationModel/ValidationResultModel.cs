using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace genesis_valida_importacao_teste.Exceptions.ValidationModel
{
    public class ValidationResultModel
    {
        public string status { get; set;}

        public List<ValidationError> Errors { get; }

        public ValidationResultModel(ModelStateDictionary modelState)
        {
            status = "400";
            Errors = modelState.Keys
                    .SelectMany(key => modelState[key].Errors.Select(x => new ValidationError(key, x.ErrorMessage)))
                    .ToList();
        }
    }
}
