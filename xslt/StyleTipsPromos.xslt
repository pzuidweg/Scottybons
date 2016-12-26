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
<xsl:variable name="styleTipsPromos" select="umbraco.library:GetXmlNodeById($currentPage/styleTipsPromos)"/>
	
<xsl:template match="/">

<!-- start writing XSLT -->
	
	<section id="our_story">
		<div class="container">
			<h3><xsl:value-of select="$currentPage/documentTitle"/></h3>
			
			<xsl:for-each select="$styleTipsPromos/descendant::*[@isDoc]">
				<xsl:sort select="styleTipsDate"  order="descending" />
				
						<div class="line">&nbsp;</div>
						<div class="row our_story_list_topspace">

							<div class="col-sm-6 ">
								<h4>
									<xsl:value-of select="./styleTipsTitle"/><br/><br/>
									<xsl:value-of select="umbraco.library:FormatDateTime(./styleTipsDate, 'dd-MM-yyyy')"/>
								</h4>
								
								
								<xsl:value-of select="./styleTipsDescription" disable-output-escaping="yes"/>
							</div>
							<div class="col-sm-6 style-tips-img">
								<xsl:variable name="mediaId" select="./styleTipsImage" />
								<xsl:if test="$mediaId != ''">
									<img src="{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}"
										 title="{$currentPage/@nodeName}"
										 alt="{$currentPage/@nodeName}"											
										 width="{umbraco.library:GetMedia($mediaId, 'false')/umbracoWidth}"
										 height="{umbraco.library:GetMedia($mediaId, 'false')/umbracoHeight}">
									</img>
								</xsl:if>

							</div> 
						</div>
					
			</xsl:for-each>	 

		</div>
	</section>
	 

</xsl:template>

</xsl:stylesheet>