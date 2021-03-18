# CalibrationDataHound
Console app to download USGS Rating Curve Data, Thin the curves down to a reasonable number of points (while maintaining the shape), and write to .csv. 
the Properties.txt file is in the format {gage number, gage name, datum, conversion to NAVD88} the conversion can be left blank is the datum is already in 88. gage number 
should not include the leading 0. Will not accept commas in the gage name. Gage name does not have to match the USGS gage name. 
