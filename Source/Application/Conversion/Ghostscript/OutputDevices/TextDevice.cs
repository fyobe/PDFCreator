﻿using pdfforge.PDFCreator.Conversion.Jobs.Jobs;
using pdfforge.PDFCreator.Utilities;
using System.Collections.Generic;
using SystemInterface.IO;

namespace pdfforge.PDFCreator.Conversion.Ghostscript.OutputDevices
{
    public class TextDevice : OutputDevice
    {
        private readonly bool _useTextDevice = true;

        public TextDevice(Job job, ConversionMode conversionMode) : base(job, conversionMode)
        {
        }

        public TextDevice(Job job, ConversionMode conversionMode, IFile file, IOsHelper osHelper, ICommandLineUtil commandLineUtil) : base(job, conversionMode, file, osHelper, commandLineUtil)
        {
        }

        protected override void AddDeviceSpecificParameters(IList<string> parameters)
        {
            if (_useTextDevice)
            {
                AddTextDeviceParameters(parameters);
            }
            else
            {
                AddPs2AsciiParameters(parameters);
            }
        }

        private void AddPs2AsciiParameters(IList<string> parameters)
        {
            //Solution with ps2ascii file, has problems with Office Open XML formats

            parameters.Add("-dNODISPLAY");
            parameters.Add("-dDELAYBIND");
            parameters.Add("-dWRITESYSTEMDICT");
            parameters.Add("-dSIMPLE"); //-dComplex is available too for output with crazy stuff^^
            parameters.Add("-q"); //Disable Ghostscript messages
            parameters.Add("ps2ascii.ps");
        }

        private void AddTextDeviceParameters(IList<string> parameters)
        {
            // Solution with gs Textdevice
            //
            // There are 4 values for dTextFormat
            // 0 and 1 output XML-escaped Unicode along with information regarding the format of the text...
            // 2/3 outputs Unicode(UCS2)/UTF-8 text which approximates the layout of the original document.
            //
            // ATTENTION:
            // TextFormat=3 would produce UTF-8, but does not work properly in GS 9.10!
            parameters.Add("-sDEVICE=txtwrite");
            var format = Job.Profile.TextSettings.Format;
            if (format < 0 || format > 3)
                format = 2; //default value
            parameters.Add("-dTextFormat=" + format);
        }

        protected override string ComposeOutputFilename()
        {
            return Job.JobTempFileName + ".txt";
        }
    }
}
