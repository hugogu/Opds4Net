using System;
using System.Xml;
using Commons.Xml.Relaxng;

namespace Opds4Net.Model.Validation
{
    /// <summary>
    /// 
    /// </summary>
    public class OpdsValidateReader
    {
        /// <summary>
        /// Don't expose this pattern, otherwise the user have to reference to the Commons.Xml.Relaxng.dll
        /// </summary>
        private RelaxngPattern relaxPattern = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rngFile"></param>
        public OpdsValidateReader(string rngFile = @".\Schemas\opds_catalog.rng")
        {
            LoadSchemaFile(rngFile);
        }

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<OpdsValidationEventArgs> ValidationError;

        /// <summary>
        /// Changet the schema file to validate Opds.
        /// </summary>
        /// <param name="rngFile"></param>
        public void SetSchemaFile(string rngFile)
        {
            LoadSchemaFile(rngFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlReader"></param>
        public void Validate(XmlReader xmlReader)
        {
            using (var reader = new RelaxngValidatingReader(xmlReader, relaxPattern))
            {
                try
                {
                    while (!reader.EOF)
                    {
                        reader.Read();
                    }
                }
                catch (RelaxngException ex)
                {
                    RaiseValidationError(ex, OpdsValidationErrorLevel.OpdsInvalid);
                }
                catch (XmlException ex)
                {
                    RaiseValidationError(ex, OpdsValidationErrorLevel.XmlInvalid);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="level"></param>
        protected virtual void RaiseValidationError(Exception ex, OpdsValidationErrorLevel level)
        {
            var temp = ValidationError;
            if (temp != null)
            {
                temp(this, new OpdsValidationEventArgs(level, ex.Message));
            }
        }

        private void LoadSchemaFile(string rngFile)
        {
            using (var rngReader = new XmlTextReader(rngFile))
            {
                relaxPattern = RelaxngPattern.Read(rngReader);
                relaxPattern.Compile();
            }
        }
    }
}
