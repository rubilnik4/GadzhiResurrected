using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GadzhiConverting.Models.Implementations.ReactiveSubjects
{
    /// <summary>
    /// Изменение состояния модели
    /// </summary>
    public class ConvertingProjectChange
    {
        public ConvertingProjectChange(ActionType actionType, 
                                       string descriptionChange)
        {
            ActionType = actionType;
            DescriptionChange = descriptionChange;
        }

        /// <summary>
        /// Тип изменения МОдели
        /// </summary>
        ActionType ActionType { get; }

        /// <summary>
        /// Описание изменения
        /// </summary>
        string DescriptionChange { get; }
    }
}
