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

	<xsl:variable name="scottyBonsPromos" select="umbraco.library:GetXmlNodeById($currentPage/scottyBonsPromos)"/>
	
<xsl:template match="/">

<!-- start writing XSLT -->
	
	
 <section id="press">
  
  <div class="container">
  <h3><xsl:value-of select="$currentPage/documentTitle"/></h3>
  <p><xsl:value-of select="$currentPage/pageDescription" disable-output-escaping="yes"/></p>
  
  <div class="press_team_line">&nbsp;</div>
  <div class="col-sm-8 col-xs-offset-2">
  <ul id="accordion" class="accordion">
	  <xsl:for-each select="$scottyBonsPromos/descendant::*[@isDoc]">
		
  <li>
    <div class="link">
		<table>
			<tr>
				<td valign="top" style="width:15px;">
					<xsl:value-of select="position()"/>.
				</td>
				<td>
					<xsl:value-of select="./joiningScottyBonsQuestion"/>
					<i class="fa fa-chevron-right">&nbsp;</i>
				</td>				
			</tr>
		</table>
	  </div>
    <ul class="submenu">
      <li>    
        <span id="accord">
          <xsl:value-of select="./joiningScottyBonsAnswer" disable-output-escaping="yes"/>
        </span>
      </li>    
 
    </ul>
  </li>
		  
</xsl:for-each>  
 
</ul>
</div>
</div>
  
  
  </section>
	
	
	
	


</xsl:template>

</xsl:stylesheet>