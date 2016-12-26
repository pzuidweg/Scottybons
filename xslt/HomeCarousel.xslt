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
	
<xsl:variable name="carouselPromos" select="umbraco.library:GetXmlNodeById($currentPage/carouselPromos)"/>

<xsl:template match="/">

<!-- start writing XSLT -->

	<section id="Outfits">			
		<div class="outfits_bg">
			<div class="col-sm-12">
				<div class="row Outfits_center_bg" >					
					<div id="owl-product" class="owl-carousel">
						
						<xsl:for-each select="$carouselPromos/descendant::*[@isDoc]">

							<div class="item">
								<div class="product-col">
									<div class="image">
										<xsl:variable name="mediaId" select="./carouselImage" />
										<xsl:if test="$mediaId != ''">
											<img src="{umbraco.library:GetMedia($mediaId, 'false')/umbracoFile}"
												 title="{$currentPage/@nodeName}"
												 alt="product"
												 class="img-responsive"
												 width="{umbraco.library:GetMedia($mediaId, 'false')/umbracoWidth}"
												 height="{umbraco.library:GetMedia($mediaId, 'false')/umbracoHeight}">
											</img>
										</xsl:if>
									</div>
									<xsl:if test="./description != ''">
									<div class="caption">

										<h4>
											<xsl:if test="./redirectionLink != ''">
												<a href="{umbraco.library:NiceUrl(./redirectionLink)}">
													<xsl:value-of select="./redirectionLinkTitle"/>
												</a>
											</xsl:if>
										</h4>
										<div class="description">
											<xsl:value-of select="./description" disable-output-escaping="yes"/>
										</div>

									</div>
										</xsl:if>
								</div>
							</div>

						</xsl:for-each>

					</div>
				</div>
			</div>
		</div>
	</section>

</xsl:template>

</xsl:stylesheet>