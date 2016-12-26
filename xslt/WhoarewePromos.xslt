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

<xsl:variable name="whoAreWePromos" select="umbraco.library:GetXmlNodeById($currentPage/whoAreWePromos)"/>
	
<xsl:template match="/">

<!-- start writing XSLT -->

	<section id="whoarewe">

		<xsl:for-each select="$whoAreWePromos/descendant::*[@isDoc]">

			<div class="container no_padding" >
				<div class="col-sm-6 mobile_display no_padding">
					<xsl:variable name="mediaId" select="./whoAreWeImage" />
					<xsl:if test="$mediaId != ''">
						<img src="{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}"
							 title="{$currentPage/@nodeName}"
							 class="whoare-img"
							 alt="{$currentPage/@nodeName}"												 
							 width="{umbraco.library:GetMedia($mediaId, 'false')/umbracoWidth}"
							 height="{umbraco.library:GetMedia($mediaId, 'false')/umbracoHeight}">
						</img>
					</xsl:if>
				</div>
				<div class="col-sm-6 col-xs-12 whoarewe-text">
					<h2 style="padding:0px">
						<xsl:value-of select="./whoAreWeHeading"/>
					</h2>

					<xsl:value-of select="./whoAreWeDescription" disable-output-escaping="yes"/>
				</div>

			</div>

			

		</xsl:for-each>
	</section>
		
</xsl:template>

</xsl:stylesheet>