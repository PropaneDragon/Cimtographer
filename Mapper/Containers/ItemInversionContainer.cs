using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mapper.Containers
{
    /// <summary>
    /// Contains a type and allows for storing to be checked later
    /// against the stoered validation type.
    /// </summary>
    /// <typeparam name="Item"></typeparam>
    class ItemInversionContainer<Item>
    {
        public enum ValidationType { None, Inverted };

        public Item storedItem_;
        public ValidationType validation_;

        public ItemInversionContainer(Item value, ValidationType validation = ValidationType.None)
        {
            storedItem_ = value;
            validation_ = validation;
        }
    }
}
