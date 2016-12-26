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
	<section id="Terms">
  <div class="container">
  <h3><xsl:value-of select="$currentPage/termsAndConditionsTitle"/></h3>
 
  
 
  <div class="col-sm-8 col-xs-offset-2 termsconditions">
  <p style="text-align:left;"><xsl:value-of select="$currentPage/termsAndConditionsDescription" disable-output-escaping="yes"/></p>
   
</div>
</div>
  
  
  </section>

</xsl:template>

</xsl:stylesheet>