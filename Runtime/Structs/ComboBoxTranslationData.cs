using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Runtime.Structs
{
    /// <summary>
    /// Translations in this struct will be done at startup and therefore will save
    /// run time operation at the cost of memory.
    /// </summary>
    internal struct ComboBoxTranslationData
    {
        #region Identity
        public const String StructName = nameof(ComboBoxTranslationData);
        #endregion /Identity

        #region Readonly
        private readonly IDictionary<Func<String>, object> dictComboBoxTranslationBase;
        private readonly ComboBoxItem[][] translations;
        #endregion /Readonly

        #region Translations
        public ComboBoxItem[][] Translations
        {
            get
            {
                return translations;
            }
        }

        [System.Runtime.CompilerServices.IndexerName("CurrentTranslation")]
        public ComboBoxItem[] this[int index] 
        {
            get
            {
                return translations[index];
            }
        }
        #endregion /Translations

        #region Constructor
        public ComboBoxTranslationData(IDictionary<Func<String>, Object> dictComboBoxTranslationBase)
        {
            this.dictComboBoxTranslationBase = dictComboBoxTranslationBase;
            translations = GenerateComboBoxItems(this.dictComboBoxTranslationBase);
        }
        #endregion /Constructor

        #region Translation Generation
        private static ComboBoxItem[][] GenerateComboBoxItems(IDictionary<Func<String>, Object> dictComboBoxTranslationBase)
        {
            ComboBoxItem[][] languageItemArray = new ComboBoxItem[Translation_Manager.LANGUAGE_COUNT][];
            try
            {
                for (int l = 0; l < Translation_Manager.LANGUAGE_COUNT; l++)
                {// For each language.
                    languageItemArray[l] = new ComboBoxItem[dictComboBoxTranslationBase.Count];
                    for (int t = 0; t < dictComboBoxTranslationBase.Count; t++)
                    {// For each element that should be in the combo box.
                        KeyValuePair<Func<String>, object> translationFunction_ReturnObject = dictComboBoxTranslationBase.ElementAt(t);
                        String translation = translationFunction_ReturnObject.Key.Invoke();
                        Object returnObject = translationFunction_ReturnObject.Value;
                        languageItemArray[l][t] = new ComboBoxItem(translation, returnObject);
                    }
                }
            }
            catch (Exception ex)
            {
                Log_Manager.LogAssert(StructName, $"Translation Combobox Array Build failed with message {ex.Message}");
            }
            return languageItemArray;
        }
        #endregion /Translation Generation
    }
}
