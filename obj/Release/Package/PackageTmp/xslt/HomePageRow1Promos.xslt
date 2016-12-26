<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE xsl:stylesheet [ <!ENTITY nbsp "&#x00A0;"> ]>
<xsl:stylesheet 
	version="1.0" 
	xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
	xmlns:msxml="urn:schemas-microsoft-com:xslt"
	xmlns:umbraco.library="urn:umbraco.library" xmlns:Examine="urn:Examine" 
	exclude-result-prefixes="msxml umbraco.library Examine ">


<xsl:output method="xml" omit-xml-declaration="yes"/>
	

<xsl:param name="currentPage"/>
	
<xsl:variable name="HomeRow1Promo" select="umbraco.library:GetXmlNodeById($currentPage/row1Promos)"/>


<xsl:template match="/">

<!-- start writing XSLT -->
	
	<div class="container">
		
		<xsl:for-each select="$HomeRow1Promo/descendant::*[@isDoc]">
			
		<div class="col-md-4 col-xs-4 tellus_mob_tab">
			<div class="box">
				<div class="row">
					<h1>
						<xsl:value-of select="./promoTitle" disable-output-escaping="yes"/>
					</h1>
					<h2>
						<xsl:value-of select="./promoSubHeading" disable-output-escaping="yes"/>
					</h2>
				</div>
				<div class="line" style="padding-top:0;">&nbsp;</div>
				<xsl:value-of select="./promoContent" disable-output-escaping="yes"/>
			</div>
		</div>
			
			</xsl:for-each>

	</div>

</xsl:template>

</xsl:stylesheet>