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
	
<xsl:variable name="rootnode" select="$currentPage/ancestor-or-self::*[@level = 2]/@id"/>
<xsl:variable name="rootnodeId" select="umbraco.library:GetXmlNodeById($rootnode)"/>
<xsl:variable name="footerPromos" select="umbraco.library:GetXmlNodeById($rootnodeId/footerPromos)"/>
	
<xsl:template match="/">

<!-- start writing XSLT -->
	<section id="footer">
		<div class="container">
			<div class="row col-xs-offset-2 tab_footer_fullwith">

				<xsl:for-each select="$footerPromos/descendant::*[@isDoc]">

					<xsl:if test="./parent::*[@isDoc]/@id = $footerPromos/@id">


						<xsl:variable name="footersectionnode" select="umbraco.library:GetXmlNodeById(./@id)"/>


						<div class=" col-sm-3 col-xs-12 col-md-4 tab_footer">
							<h3>
								<xsl:value-of select="@nodeName"/>


							</h3>
							<ul class="list-unstyled">
								<xsl:for-each select="$footersectionnode/descendant::*[@isDoc]">
									<li>
										<a>
											<xsl:if test="string(./isExternalWindow) = 1">
												<xsl:attribute name="target">
													<xsl:value-of select="'_blank'" />
												</xsl:attribute>
											</xsl:if>

											<xsl:choose>
											   <xsl:when test="./footerRedirectionLink != ''">
												<xsl:attribute name="href">
												 	<xsl:value-of select="umbraco.library:NiceUrl(./footerRedirectionLink)" />
												</xsl:attribute>
											   </xsl:when>
											   <xsl:when test="./footerRedirectionExtLink != ''">
												<xsl:attribute name="href">
												 <xsl:value-of select="./footerRedirectionExtLink" />
												</xsl:attribute>
											   </xsl:when>
											  </xsl:choose>

											<xsl:value-of select="./footerLinkTitle"/>
										</a>
									</li>
								</xsl:for-each>	

							</ul>
						</div>
					</xsl:if>
				</xsl:for-each>	
			</div>
		</div>
	</section>
<!-- starting footer -->
<section id="footerendingline"> &nbsp;</section>

</xsl:template>

</xsl:stylesheet>