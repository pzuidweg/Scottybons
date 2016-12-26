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
	
<xsl:variable name="HomeRow2Promo" select="umbraco.library:GetXmlNodeById($currentPage/row2Promo)"/>


<xsl:template match="/">

<!-- start writing XSLT -->
	
	<div class="container">
		<xsl:for-each select="$HomeRow2Promo/descendant::*[@isDoc]">
			<div class="row" id="What-does-it-cost-bg">


				<div class="col-sm-4 whatdoseitcost">
          <div class="row">
					<h2><xsl:value-of select="./promoSubHeading"/></h2>
          </div>
				</div>
				<div class="col-sm-8 " style="padding:0px;">
					<xsl:value-of select="./promoContent" disable-output-escaping="yes"/>
				</div>
			</div>

		</xsl:for-each>
	</div>
	
	
	

</xsl:template>

</xsl:stylesheet>