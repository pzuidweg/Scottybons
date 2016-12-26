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

	<xsl:variable name="pressQuestionPromos" select="umbraco.library:GetXmlNodeById($currentPage/pressQuestionPromos)"/>
	<xsl:variable name="pressImagePromos" select="umbraco.library:GetXmlNodeById($currentPage/pressImagePromos)"/>
	
	<xsl:template match="/">

		<!-- start writing XSLT -->


		<section id="press">
			<div class="container">
				<xsl:if test="$currentPage/heading != ''">
					<h3><xsl:value-of select="$currentPage/heading"/></h3>
				</xsl:if>

				<xsl:if test="$currentPage/subHeading != ''">
					<p><xsl:value-of select="$currentPage/subHeading"/></p>
				</xsl:if>


				<div class="row press_img">
					<xsl:for-each select="$pressImagePromos/descendant::*[@isDoc]">
						<xsl:if test="position() mod 4 = 1">
							<xsl:call-template name="showimages">
								<xsl:with-param name="position" select = "position()" />
							</xsl:call-template>

						</xsl:if>
					</xsl:for-each>




				</div>


				<div class="col-sm-10 col-xs-offset-1 mobile_width">
					<ul id="accordion" class="accordion">
						<xsl:for-each select="$pressQuestionPromos/descendant::*[@isDoc]">


							<li>
								<div class="link">									
									<table>
										<tr>
											<td valign="top" style="width:15px;">
												<xsl:value-of select="position()"/>.
											</td>
											<td>
												<xsl:value-of select="./pressQuestion"/>
												<i class="fa fa-chevron-right">&nbsp;</i>
											</td>				
										</tr>
									</table>								
									
								</div>
								<ul class="submenu">
                  <li>
                    <span id="accord">
                      <xsl:value-of select="./pressAnswer" disable-output-escaping="yes"/>
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

	<xsl:template name = "showimages" >
		<xsl:param name = "position" />

		<ul>
			<xsl:for-each select="$pressImagePromos/descendant::*[@isDoc]">
				<xsl:if test="position() &gt;= $position and position() &lt; $position+4">

					<li> 

						<xsl:variable name="mediaId" select="(./pressImage)" />
						<xsl:if test="$mediaId != ''">

							<xsl:choose>
								<xsl:when test="./pressRedirectionLink != ''">
									<a href="{umbraco.library:NiceUrl(./pressRedirectionLink)}" >

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
				</xsl:if>

			</xsl:for-each>
		</ul>
	</xsl:template>
	
	
</xsl:stylesheet>