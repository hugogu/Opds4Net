﻿using System;
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
        /// 
        /// </summary>
        public event EventHandler<OpdsValidationEventArgs> ValidationError;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlFile"></param>
        public void Validate(string xmlFile, string rngFile = @".\Schemas\opds_catalog.rng")
        {
            RelaxngPattern relaxPattern = null;
            using (var rngReader = new XmlTextReader(rngFile))
            {
                relaxPattern = RelaxngPattern.Read(rngReader);
                relaxPattern.Compile();
            }

            using (var reader = new RelaxngValidatingReader(new XmlTextReader(xmlFile), relaxPattern))
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
                    RaiseValidationError(ex, OpdsValidationErrorLevel.OpdsInvalid, xmlFile);
                }
                catch (XmlException ex)
                {
                    RaiseValidationError(ex, OpdsValidationErrorLevel.XmlInvalid, xmlFile);
                }
            }
        }

        protected virtual void RaiseValidationError(Exception e, OpdsValidationErrorLevel level, string file)
        {
            var temp = ValidationError;
            if (temp != null)
            {
                temp(this, new OpdsValidationEventArgs(level, e.Message, file));
            }
        }
    }
}