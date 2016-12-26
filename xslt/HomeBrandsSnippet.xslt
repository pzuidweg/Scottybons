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
	
<xsl:variable name="BrandsPromos" select="umbraco.library:GetXmlNodeById($currentPage/brandsPromos)"/>

<xsl:template match="/">

<!-- start writing XSLT -->
	
	<section id="brands">
		<div class="container m_width">
			<ul>
				<xsl:for-each select="$BrandsPromos/descendant::*[@isDoc]">

					<li> 
						<xsl:variable name="mediaId" select="(./brandImage)" />
						<xsl:if test="$mediaId != ''">
							
							<xsl:choose>
								<xsl:when test="./redirectionLink != ''">
									<a href="{umbraco.library:NiceUrl(./redirectionLink)}" >

										<img src="{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}"
											 title="{$currentPage/@nodeName}"
											 alt="{$currentPage/@nodeName}"
											 width="{umbraco.library:GetMedia($mediaId, 'false')/umbracoWidth}"
											 height="{umbraco.library:GetMedia($mediaId, 'false')/umbracoHeight}">
										</img>
									</a>
								</xsl:when>
								<xsl:otherwise>
									<img src="{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}"
										 title="{$currentPage/@nodeName}"
										 alt="{$currentPage/@nodeName}"
										 width="{umbraco.library:GetMedia($mediaId, 'false')/umbracoWidth}"
										 height="{umbraco.library:GetMedia($mediaId, 'false')/umbracoHeight}">
									</img>
								</xsl:otherwise>
							</xsl:choose>						

						</xsl:if>
					</li>


				</xsl:for-each>
			</ul>
		</div>
	</section>

</xsl:template>

</xsl:stylesheet>