using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.IO;

namespace StorageMonster.Web.Services
{
    public class IconProvider : IIconProvider
    {
        private static readonly IDictionary<string, string> IconNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        private const string DefaultFile = "application_x_zerosize.png";
        private const string DefaultFolder = "folder.png";

        private const string MimeIconPathFormat = "~/Content/themes/base/icons22x22/{0}";
        private const string ImagePathFormat = "~/Content/themes/base/images/{0}";

        public string GetImagePath(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            return string.Format(CultureInfo.InvariantCulture, ImagePathFormat, name);
        }

        public string GetIconPath(string fileName, ItemType type)
        {
            switch (type)
            {
                case ItemType.Folder:
                    return string.Format(CultureInfo.InvariantCulture, MimeIconPathFormat, DefaultFolder);
                default:
                    string extension = Path.GetExtension(fileName);
                    string iconName = IconNames.Keys.Contains(extension) ? IconNames[extension] : DefaultFile;
                    return string.Format(CultureInfo.InvariantCulture, MimeIconPathFormat, iconName);
            }
        }

        public void Initizlize()
        {
            IconNames.Add(".pdf", "application_pdf.png");
            IconNames.Add(".djvu", "image_x_generic.png");
            IconNames.Add(".nfo", "text_x_texinfo.png");
            IconNames.Add(".txt", "text_x_generic.png");
            IconNames.Add(".csv", "text_csv.png");
            IconNames.Add(".rtf", "application_rtf.png");
                        
            IconNames.Add(".zip", "application_x_zip.png");
            IconNames.Add(".7z", "application_x_7z_compressed.png");
            IconNames.Add(".tar", "application_x_tar.png");
            IconNames.Add(".gz", "application_x_tarz.png");
            IconNames.Add(".bz", "application_x_bzip.png");
            IconNames.Add(".bz2", "application_x_bzip.png");
            IconNames.Add(".rar", "application_x_rar.png");
            IconNames.Add(".arc", "application_x_arc.png");
            IconNames.Add(".arj", "application_x_arj.png");
            IconNames.Add(".ace", "application_x_ace.png");
            IconNames.Add(".lha", "application_x_lha.png");
            IconNames.Add(".lzo", "application_x_lzop.png");
          
            IconNames.Add(".xls", "application_vnd.ms_excel.png");
            IconNames.Add(".xlsx", "application_vnd.ms_excel.png");
            IconNames.Add(".doc", "application_vnd.ms_word.png");
            IconNames.Add(".docx", "application_vnd.ms_word.png");
            IconNames.Add(".ppsx", "application_vnd.ms_powerpoint.png");
            IconNames.Add(".pps", "application_vnd.ms_powerpoint.png");
            IconNames.Add(".odf", "formula.png");
            IconNames.Add(".odt", "document.png");
            IconNames.Add(".ods", "application_x_applix_spreadsheet.png");
            IconNames.Add(".odp", "presentation.png");
            IconNames.Add(".odg", "drawing.png");

            IconNames.Add(".exe", "application_x_executable.png");
            IconNames.Add(".dll", "binary.png");
            IconNames.Add(".com", "application_x_ms_dos_executable.png");
            IconNames.Add(".so", "binary.png");
            IconNames.Add(".msi", "msi.png");
            IconNames.Add(".jar", "application_x_java_archive.png");
            IconNames.Add(".o", "application_x_object.png");
            IconNames.Add(".obj", "application_x_object.png");
            
            IconNames.Add(".sh", "application_x_executable_script.png");
            IconNames.Add(".cmd", "application_x_executable_script.png");
            IconNames.Add(".bat", "application_x_executable_script.png");
            IconNames.Add(".awk", "application_x_awk.png");            
            IconNames.Add(".make", "text_x_makefile.png");
            
            IconNames.Add(".cs", "text_x_csharp.png");
            IconNames.Add(".java", "text_x_java.png");
            IconNames.Add(".py", "text_x_python.png");
            IconNames.Add(".c", "source_c.png");
            IconNames.Add(".cpp", "source_cpp.png");
            IconNames.Add(".h", "source_h.png");
            IconNames.Add(".hpp", "source_h.png");
            IconNames.Add(".pl", "application_x_perl.png");
            IconNames.Add(".php", "application_x_php.png");
            IconNames.Add(".js", "application_x_javascript.png");            
            IconNames.Add(".rb", "application_x_ruby.png");         
            IconNames.Add(".xml", "application_xml.png");
            IconNames.Add(".html", "html.png");           
            IconNames.Add(".css", "text_css.png");    
            IconNames.Add(".xhtml", "application_xhtml+xml.png");  
            IconNames.Add(".xsd", "application_xsd.png");
            IconNames.Add(".xslt", "application_xslt+xml.png");
            IconNames.Add(".sql", "text_x_sql.png");

            IconNames.Add(".iso", "application_x_cd_image.png");
            IconNames.Add(".isz", "application_x_cd_image.png");
            IconNames.Add(".nrg", "application_x_cd_image.png");
            IconNames.Add(".cue", "application_x_cue.png");
            
            IconNames.Add(".deb", "application_x_deb.png");
            IconNames.Add(".rpm", "application_x_rpm.png");
            
            IconNames.Add(".bmp", "image_bmp.png");
            IconNames.Add(".gif", "image_gif.png");
            IconNames.Add(".png", "image_png.png");
            IconNames.Add(".tiff", "image_tiff.png");
            IconNames.Add(".tga", "image_x_tga.png");
            IconNames.Add(".jpg", "image_jpeg.png");
            IconNames.Add(".jpeg", "image_jpeg.png");
            IconNames.Add(".ico", "image_x_ico.png");
            IconNames.Add(".psd", "image_x_psd.png");
            IconNames.Add(".dds", "image_x_dds.png");
            IconNames.Add(".svg", "image_svg+xml.png");
            IconNames.Add(".xpm", "image_x_xpixmap.png");
                        
            IconNames.Add(".aac", "audio_aac.png");
            IconNames.Add(".mp3", "audio_mpeg.png");
            IconNames.Add(".flac", "audio_x_flac.png");
            IconNames.Add(".aiff", "audio_x_aiff.png");
            IconNames.Add(".wav", "audio_x_wav.png");
            IconNames.Add(".wma", "audio_x_ms_wma.png");
            IconNames.Add(".ac3", "audio_ac3.png");
            IconNames.Add(".midi", "audio_midi.png");
            IconNames.Add(".ra", "audio_vnd.rn_realaudio.png");
            IconNames.Add(".ram", "audio_vnd.rn_realaudio.png");
            IconNames.Add(".ape", "audio_x_monkey.png");
            IconNames.Add(".mpc", "audio_x_musepack.png");
            IconNames.Add(".mp+", "audio_x_musepack.png");
            IconNames.Add(".mpp", "audio_x_musepack.png");

            IconNames.Add(".mp4", "video_mp4.png");
            IconNames.Add(".mkv", "video_x_matroska.png");
            IconNames.Add(".wmv", "video_x_ms_wmv.png");
            IconNames.Add(".avi", "video_x_msvideo.png");
            IconNames.Add(".asf", "video_x_ms_asf.png");
            IconNames.Add(".ogm", "video_x_ogm+ogg.png");
            IconNames.Add(".rm", "audio_vnd.rn_realvideo.png");
            IconNames.Add(".flv", "flv.png");
            
            IconNames.Add(".torrent", "application_x_bittorrent.png");
            IconNames.Add(".rss", "application_rss+xml.png");
            
            IconNames.Add(".afm", "application_x_font_afm.png");
            IconNames.Add(".bdf", "application_x_font_bdf.png");
            IconNames.Add(".otf", "application_x_font_otf.png");
            IconNames.Add(".pcf", "application_x_font_pcf.png");
            IconNames.Add(".snf", "application_x_font_snf.png");
            IconNames.Add(".ttf", "application_x_font_ttf.png");            
        }
    }
}