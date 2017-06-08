<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns="http://www.w3.org/TR/xhtml1/strict">
  <xsl:output method="html"/>
  <xsl:template match="/">
    <html>
      <head>
        <style media="screen"
               type="text/css">
          body { font-family: Verdana }
          .module { text-transform:uppercase; }
          .namespace { font-weight: bold; }
          .type { font-weight: bold; }
          .error { color: white; background-color: red; }
          .warn { color: black; background-color: yellow; }
        </style>
      </head>
      <body>
        <table class="section-table"
               cellpadding="2"
               cellspacing="0"
               border="0"
               width="98%">
          <tr>
            <td class="sectionheader"
                colspan="9">
              Microsoft Metrics
            </td>
          </tr>
          <tr>
            <th colspan="4">Item</th>
            <th>
              Maintainability<br/>Index
            </th>
            <th>
              Cyclomatic<br/>Complexity
            </th>
            <th>
              Class<br/>Coupling
            </th>
            <th>
              Inheritance<br/>Depth
            </th>
            <th>
              Lines<br/>Of Code
            </th>
          </tr>
          <xsl:apply-templates select="//CodeMetricsReport/Targets/Target/Modules/Module">
            <xsl:sort select="@Name"/>
          </xsl:apply-templates>
        </table>
      </body>
    </html>
  </xsl:template>

  <xsl:template match="Metrics">
    <xsl:variable name="mIndex"
                  select="Metric[@Name='MaintainabilityIndex']/@Value"/>
    <xsl:variable name="cComp"
                  select="Metric[@Name='CyclomaticComplexity']/@Value"/>
    <xsl:variable name="cCoup"
                  select="Metric[@Name='ClassCoupling']/@Value"/>
    <xsl:variable name="doi"
                  select="Metric[@Name='DepthOfInheritance']/@Value"/>
    <xsl:variable name="loc"
                  select="Metric[@Name='LinesOfCode']/@Value"/>

    <td align="center">
      <xsl:if test="not( $doi )">
      <xsl:choose>
        <xsl:when test="$mIndex &lt; 40">
          <xsl:attribute name="class">error</xsl:attribute>
        </xsl:when>
        <xsl:when test="$mIndex &lt; 60">
          <xsl:attribute name="class">warn</xsl:attribute>
        </xsl:when>
      </xsl:choose>
      <xsl:value-of select="$mIndex"/>
      </xsl:if>
    </td>

    <td align="center">
      <xsl:if test="not( $doi )">
        <xsl:choose>
          <xsl:when test="$cComp &gt; 15">
            <xsl:attribute name="class">error</xsl:attribute>
          </xsl:when>
          <xsl:when test="$cComp &gt; 10">
            <xsl:attribute name="class">warn</xsl:attribute>
          </xsl:when>
        </xsl:choose>
        <xsl:value-of select="$cComp"/>
      </xsl:if>
    </td>

    <td align="center">
      <xsl:choose>
        <xsl:when test="$cCoup &gt; 20">
          <xsl:attribute name="class">error</xsl:attribute>
        </xsl:when>
      </xsl:choose>
      <xsl:value-of select="$cCoup"/>
    </td>

    <td align="center">
      <xsl:value-of select="$doi"/>
    </td>

    <td align="center">
      <xsl:value-of select="$loc"/>
    </td>
  </xsl:template>

  <xsl:template match="Module">
    <tr>
      <td colspan="4" class="module">
        <xsl:value-of select="@Name"/>
      </td>
    </tr>
    <xsl:apply-templates select="Namespaces/Namespace">
      <xsl:sort select="@Name"/>
    </xsl:apply-templates>
  </xsl:template>

  <xsl:template match="Namespace">
    <tr>
      <td>
        <span style="width:10px;"/>
      </td>
      <td colspan="3" class="namespace">
        <xsl:value-of select="@Name"/>
      </td>
    </tr>
    <xsl:apply-templates select="Types/Type">
      <xsl:sort select="@Name"/>
    </xsl:apply-templates>
  </xsl:template>

  <xsl:template match="Type">
    <tr>
      <td>
        <span style="width:10px;"/>
      </td>
      <td>
        <span style="width:10px;"/>
      </td>
      <td colspan="2" class="type">
        <xsl:value-of select="@Name"/>
      </td>
      <xsl:apply-templates select="Metrics" />
    </tr>
    <xsl:apply-templates select="Members/Member">
      <xsl:sort select="@Name"/>
    </xsl:apply-templates>
  </xsl:template>

  <xsl:template match="Member">
    <tr>
      <td>
        <span style="width:10px;"/>
      </td>
      <td>
        <span style="width:10px;"/>
      </td>
      <td>
        <span style="width:10px;"/>
      </td>
      <td>
        <xsl:value-of select="@Name"/>
      </td>
      <xsl:apply-templates select="Metrics" />
    </tr>
  </xsl:template>

</xsl:stylesheet>
