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

<xsl:template match="/">

<!-- start writing XSLT -->
	
	<xsl:variable name="rootnode" select="$currentPage/ancestor-or-self::*[@level = 2]/@id"/>
	
	<xsl:variable name="TCnode" >
		<xsl:choose>
			<xsl:when test="$rootnode = 1403">
				<xsl:value-of select="1800"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="1803"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:variable>
	
	<xsl:variable name="TermsandConditions" select="umbraco.library:GetXmlNodeById($TCnode)"/>
		
	

	
	 <!--<div id="openModal" class="modalDialog">
        <div>
            <a href="#close" title="Close" class="close">X</a>
           <h3><xsl:value-of select="$TermsandConditions/termsAndConditionsTitle"/></h3>
            <p style="text-align:center">
				<xsl:value-of select="$TermsandConditions/termsAndConditionsDescription" disable-output-escaping="yes"/>
			</p>
           
        </div>
    </div>-->

</xsl:template>

</xsl:stylesheet>